// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NEL.MESH.Clients;
using NEL.MESH.Models.Configurations;
using Tynamix.ObjectFiller;
using WireMock.Server;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class MeshClientTests
    {
        private readonly MeshClient meshClient;
        private readonly WireMockServer wireMockServer;
        private readonly MeshConfiguration meshConfigurations;

        public MeshClientTests()
        {
            this.wireMockServer = WireMockServer.Start();
            this.meshConfigurations = new MeshConfiguration
            {
                ClientCertificate = new X509Certificate2(new byte[0]),
                IntermediateCertificates = new X509Certificate2Collection(),
                MailboxId = GetRandomString(),
                MexClientVersion = GetRandomString(),
                MexOSName = GetRandomString(),
                MexOSVersion = GetRandomString(),
                Password = GetRandomString(),
                RootCertificate = new X509Certificate2(new byte[0]),
                Url = $"https://{GetRandomString()}.com"
            };

            this.meshClient = new MeshClient(meshConfigurations: this.meshConfigurations);
        }

        private string GenerateAuthorisationHeader()
        {
            string mailboxId = this.meshConfigurations.MailboxId;
            string password = this.meshConfigurations.Password;
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

        private static string GetRandomString() =>
            new MnemonicString(wordCount: 1, wordMinLength: 1, wordMaxLength: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
