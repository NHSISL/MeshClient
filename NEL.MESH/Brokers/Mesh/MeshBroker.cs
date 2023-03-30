// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NEL.MESH.Models.Configurations;

namespace NEL.MESH.Brokers.Mesh
{
    internal class MeshBroker : IMeshBroker
    {
        private readonly HttpClient httpClient;

        public MeshBroker(MeshConfiguration MeshConfiguration)
        {
            this.MeshConfiguration = MeshConfiguration;
            this.httpClient = SetupHttpClient();
        }

        public MeshConfiguration MeshConfiguration { get; private set; }

        public async ValueTask<HttpResponseMessage> HandshakeAsync()
        {
            string path = $"/messageexchange/{this.MeshConfiguration.MailboxId}";
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> SendMessageAsync(
            string mailboxTo,
            string workflowId,
            string localId,
            string subject,
            string fileName,
            string contentChecksum,
            string contentEncrypted,
            string encoding,
            string chunkRange,
            string contentType,
            string authorizationToken,
            string stringConent)
        {
            var path = $"/messageexchange/{this.MeshConfiguration.MailboxId}/outbox";

            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = new StringContent(stringConent, Encoding.UTF8, contentType)
            };

            request.Headers.Add("Mex-From", this.MeshConfiguration.MailboxId);
            request.Headers.Add("Mex-To", mailboxTo);
            request.Headers.Add("Mex-WorkflowID", workflowId);
            request.Headers.Add("Mex-LocalID", localId);
            request.Headers.Add("Mex-Subject", subject);
            request.Headers.Add("Mex-FileName", fileName);
            request.Headers.Add("Mex-Content-Checksum", contentChecksum);
            request.Headers.Add("Mex-Content-Encrypted", contentEncrypted);
            request.Headers.Add("Mex-Encoding", encoding);
            request.Headers.Add("Mex-Chunk-Range", chunkRange);
            request.Headers.Add("Authorization", authorizationToken);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> SendFileAsync(
            string mailboxTo,
            string workflowId,
            string localId,
            string subject,
            string fileName,
            string contentChecksum,
            string contentEncrypted,
            string encoding,
            string chunkRange,
            string contentType,
            string authorizationToken,
            byte[] fileContents)
        {

            var path = $"/messageexchange/{this.MeshConfiguration.MailboxId}/outbox";
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Headers.Add("Authorization", authorizationToken);
            request.Content = new ByteArrayContent(fileContents);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            request.Content.Headers.Add("Mex-From", this.MeshConfiguration.MailboxId);
            request.Content.Headers.Add("Mex-To", mailboxTo);
            request.Content.Headers.Add("Mex-WorkflowID", workflowId);
            request.Content.Headers.Add("Mex-LocalID", localId);
            request.Content.Headers.Add("Mex-Subject", subject);
            request.Content.Headers.Add("Mex-FileName", fileName);
            request.Content.Headers.Add("Mex-Content-Checksum", contentChecksum);
            request.Content.Headers.Add("Mex-Content-Encrypted", contentEncrypted);
            request.Content.Headers.Add("Mex-Encoding", encoding);
            request.Content.Headers.Add("Mex-Chunk-Range", chunkRange);

            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> TrackMessageAsync(string messageId, string authorizationToken)
        {
            var path = $"/messageexchange/{this.MeshConfiguration.MailboxId}/outbox/tracking?messageID={messageId}";
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Authorization", authorizationToken);
            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> GetMessagesAsync(string authorizationToken)
        {
            var path = $"/messageexchange/{this.MeshConfiguration.MailboxId}/inbox";
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Authorization", authorizationToken);
            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> GetMessageAsync(string messageId, string authorizationToken)
        {
            var path = $"/messageexchange/{this.MeshConfiguration.MailboxId}/inbox/{messageId}";
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Authorization", authorizationToken);
            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        public async ValueTask<HttpResponseMessage> AcknowledgeMessageAsync(string messageId, string authorizationToken)
        {
            var path = $"/messageexchange/{this.MeshConfiguration.MailboxId}/inbox/{messageId}/status/acknowledged";
            var request = new HttpRequestMessage(HttpMethod.Put, path);
            request.Headers.Add("Authorization", authorizationToken);
            var response = await this.httpClient.SendAsync(request);

            return response;
        }

        private HttpClient SetupHttpClient()
        {
            HttpClientHandler handler = SetupHttpClientHandler();

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(this.MeshConfiguration.Url)
            };

            httpClient.DefaultRequestHeaders.Add(
                name: "Mex-ClientVersion",
                value: this.MeshConfiguration.MexClientVersion);

            httpClient.DefaultRequestHeaders.Add(
                name: "Mex-OSName",
                value: this.MeshConfiguration.MexOSName);

            httpClient.DefaultRequestHeaders.Add(
                name: "Mex-OSVersion",
                value: this.MeshConfiguration.MexOSVersion);

            return httpClient;
        }

        private HttpClientHandler SetupHttpClientHandler()
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                CheckCertificateRevocationList = false,
            };

            if (this.MeshConfiguration.ClientCertificate != null)
            {
                handler.ClientCertificates.Add(this.MeshConfiguration.ClientCertificate);
            }

            if (this.MeshConfiguration.RootCertificate != null)
            {
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (chain != null)
                    {
                        chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                        chain.ChainPolicy.CustomTrustStore.Add(this.MeshConfiguration.RootCertificate);

                        if (this.MeshConfiguration.IntermediateCertificates != null)
                        {
                            chain.ChainPolicy.ExtraStore.AddRange(this.MeshConfiguration.IntermediateCertificates);
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

        ~MeshBroker()
        {
            this.httpClient.Dispose();
        }
    }
}
