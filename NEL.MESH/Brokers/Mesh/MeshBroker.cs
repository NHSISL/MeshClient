// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NEL.MESH.Models.Configurations;

namespace NEL.MESH.Brokers.Mesh
{
    internal class MeshBroker : IMeshBroker
    {
        private readonly IMeshApiConfiguration MeshApiConfiguration;
        private readonly HttpClient httpClient;

        internal MeshBroker(IMeshApiConfiguration meshApiConfiguration)
        {
            this.MeshApiConfiguration = meshApiConfiguration;
            this.httpClient = SetupHttpClient();
        }

        public async ValueTask<HttpResponseMessage> HandshakeAsync()
        {
            string path = $"/messageexchange/{this.MeshApiConfiguration.MailboxId}";
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Authorization", GenerateAuthorisationHeader());
            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> SendMessageAsync(
            string mailboxTo,
            string workflowId,
            string message,
            string contentType)
        {
            var path = $"/messageexchange/{this.MeshApiConfiguration.MailboxId}/outbox";
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Headers.Add("Authorization", GenerateAuthorisationHeader());
            request.Content = new StringContent(message);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            request.Content.Headers.Add("Mex-From", this.MeshApiConfiguration.MailboxId);
            request.Content.Headers.Add("Mex-To", mailboxTo);
            request.Content.Headers.Add("Mex-WorkflowID", workflowId);
            request.Content.Headers.Add("Mex-LocalID", Guid.NewGuid().ToString());
            request.Content.Headers.Add("Content-Type", contentType);

            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> SendFileAsync(
            string mailboxTo,
            string workflowId,
            string contentType,
            byte[] fileContents,
            string fileName)
        {
            var stream = new MemoryStream(fileContents);
            var content = new ByteArrayContent(stream.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Headers.Add("Mex-From", this.MeshApiConfiguration.MailboxId);
            content.Headers.Add("Mex-To", mailboxTo);
            content.Headers.Add("Mex-WorkflowID", workflowId);
            content.Headers.Add("Mex-LocalID", Guid.NewGuid().ToString());
            content.Headers.Add("Mex-FileName", fileName);
            content.Headers.Add("Content-Type", contentType);

            var response = await this.httpClient
                .PostAsync($"/messageexchange/{this.MeshApiConfiguration.MailboxId}/outbox", content);

            return response;
        }

        public async ValueTask<HttpResponseMessage> TrackMessageAsync(string messageId)
        {
            var path = $"/messageexchange/{this.MeshApiConfiguration.MailboxId}/outbox/tracking?messageID={messageId}";
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Authorization", GenerateAuthorisationHeader());
            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> GetMessagesAsync()
        {
            var path = $"/messageexchange/{this.MeshApiConfiguration.MailboxId}/inbox";
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Authorization", GenerateAuthorisationHeader());
            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> GetMessageAsync(string messageId)
        {
            var path = $"/messageexchange/{this.MeshApiConfiguration.MailboxId}/inbox/{messageId}";
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Authorization", GenerateAuthorisationHeader());
            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> AcknowledgeMessageAsync(string messageId)
        {
            var path = $"/messageexchange/{this.MeshApiConfiguration.MailboxId}/inbox/{messageId}/status/acknowledged";
            var request = new HttpRequestMessage(HttpMethod.Put, path);
            request.Headers.Add("Authorization", GenerateAuthorisationHeader());
            var response = await this.httpClient.SendAsync(request);

            return response;
        }


        private HttpClient SetupHttpClient()
        {
            HttpClientHandler handler = SetupHttpClientHandler();

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(this.MeshApiConfiguration.Url)
            };

            httpClient.DefaultRequestHeaders.Add(
                name: "Mex-ClientVersion",
                value: this.MeshApiConfiguration.MexClientVersion);

            httpClient.DefaultRequestHeaders.Add(
                name: "Mex-OSName",
                value: this.MeshApiConfiguration.MexOSName);

            httpClient.DefaultRequestHeaders.Add(
                name: "Mex-OSVersion",
                value: this.MeshApiConfiguration.MexOSVersion);

            return httpClient;
        }

        private HttpClientHandler SetupHttpClientHandler()
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                CheckCertificateRevocationList = false
            };

            if (this.MeshApiConfiguration.ClientCertificate != null)
            {
                handler.ClientCertificates.Add(this.MeshApiConfiguration.ClientCertificate);
            }

            if (this.MeshApiConfiguration.RootCertificate != null)
            {
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (chain != null)
                    {
                        chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                        chain.ChainPolicy.CustomTrustStore.Add(this.MeshApiConfiguration.RootCertificate);
                        if (this.MeshApiConfiguration.IntermediateCertificates != null)
                        {
                            chain.ChainPolicy.ExtraStore.AddRange(this.MeshApiConfiguration.IntermediateCertificates);
                        }

                        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.IgnoreWrongUsage;

                        if (cert != null && chain.Build(cert))
                        {
                            return true;
                        };
                    }

                    throw new Exception(chain.ChainStatus.FirstOrDefault().StatusInformation);
                };
            }

            return handler;
        }

        private string GenerateAuthorisationHeader()
        {
            string mailboxId = this.MeshApiConfiguration.MailboxId;
            string password = this.MeshApiConfiguration.Password;
            string nonce = Guid.NewGuid().ToString();
            string timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmm");
            string nonce_count = "0";
            string stringToHash = $"{mailboxId}:{nonce}:{nonce_count}:{password}:{timeStamp}";
            string key = "BackBone";
            string sharedKey = GenerateSha256(stringToHash, key);

            return $"NHSMESH {mailboxId}:{nonce}:{nonce_count}:{timeStamp}:{sharedKey}";
        }

        private string GenerateSha256(string value, string key)
        {
            var crypt = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(value));

            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }

            return hash;
        }

        ~MeshBroker()
        {
            this.httpClient.Dispose();
        }
    }
}
