// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http;
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

        public MeshBroker(IMeshApiConfiguration meshApiConfiguration)
        {
            this.MeshApiConfiguration = meshApiConfiguration;
            this.httpClient = SetupHttpClient();
        }

        public async Task<HttpResponseMessage> HandshakeAsync()
        {
            string path = $"/messageexchange/{this.MeshApiConfiguration.MailboxId}";

            HttpResponseMessage response =
                await httpClient.GetAsync($"/messageexchange/{this.MeshApiConfiguration.MailboxId}");

            return response;
        }

        public Task<HttpResponseMessage> SendMessageAsync(string message, string mailboxTo, string workflowId) =>
            throw new NotImplementedException();

        public Task<HttpResponseMessage> SendFileAsync(byte[] fileContents, string mailboxTo, string workflowId) =>
            throw new NotImplementedException();

        public Task<HttpResponseMessage> TrackMessageAsync(string messageId) =>
            throw new NotImplementedException();

        public Task<HttpResponseMessage> AcknowledgeMessageAsync(string messageId) =>
            throw new NotImplementedException();

        public Task<HttpResponseMessage> GetMessageAsync(string messageId) =>
            throw new NotImplementedException();

        public Task<HttpResponseMessage> GetMessagesAsync() =>
            throw new NotImplementedException();


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
