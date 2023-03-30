// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
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
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Headers.Add("Authorization", authorizationToken);
            request.Content = new StringContent(stringConent);
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
            var stream = new MemoryStream(fileContents);
            var content = new ByteArrayContent(stream.ToArray());
            content.Headers.Add("Authorization", authorizationToken);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            content.Headers.Add("Mex-From", this.MeshConfiguration.MailboxId);
            content.Headers.Add("Mex-To", mailboxTo);
            content.Headers.Add("Mex-WorkflowID", workflowId);
            content.Headers.Add("Mex-LocalID", localId);
            content.Headers.Add("Mex-Subject", subject);
            content.Headers.Add("Mex-FileName", fileName);
            content.Headers.Add("Mex-Content-Checksum", contentChecksum);
            content.Headers.Add("Mex-Content-Encrypted", contentEncrypted);
            content.Headers.Add("Mex-Encoding", encoding);
            content.Headers.Add("Mex-Chunk-Range", chunkRange);

            var response = await this.httpClient
                .PostAsync($"/messageexchange/{this.MeshConfiguration.MailboxId}/outbox", content);

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
