// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Configuration;
using NEL.MESH.Clients;
using NEL.MESH.Models.Configurations;
using NEL.MESH.Models.Foundations.Mesh;
using Tynamix.ObjectFiller;

namespace NEL.MESH.Tests.Integration
{
    public partial class MeshClientTests
    {
        private readonly MeshClient meshClient;
        private readonly MeshConfiguration meshConfigurations;

        public MeshClientTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("NEL_MESH_CLIENT_INTEGRATION_");

            IConfiguration configuration = configurationBuilder.Build();
            bool RunAcceptanceTests = configuration.GetSection("RunAcceptanceTests").Get<bool>();
            bool RunIntegrationTests = configuration.GetSection("RunIntegrationTests").Get<bool>();
            var mailboxId = configuration["MeshConfiguration:MailboxId"];
            var mexClientVersion = configuration["MeshConfiguration:MexClientVersion"];
            var mexOSName = configuration["MeshConfiguration:MexOSName"];
            var mexOSVersion = configuration["MeshConfiguration:MexOSVersion"];
            var password = configuration["MeshConfiguration:Password"];
            var key = configuration["MeshConfiguration:Key"];
            var clientCert = configuration["MeshConfiguration:ClientCertificate"];
            var rootCert = configuration["MeshConfiguration:RootCertificate"];
            var url = configuration["MeshConfiguration:Url"];
            var maxChunkSizeInMegabytes = int.Parse(configuration["MeshConfiguration:MaxChunkSizeInMegabytes"]);

            var intermediateCertificates =
                configuration.GetSection("MeshConfiguration:IntermediateCertificates")
                    .Get<List<string>>();

            this.meshConfigurations = new MeshConfiguration
            {
                MailboxId = mailboxId,
                MexClientVersion = mexClientVersion,
                MexOSName = mexOSName,
                MexOSVersion = mexOSVersion,
                Password = password,
                Key = key,
                RootCertificate = GetCertificate(rootCert),
                IntermediateCertificates = GetCertificates(intermediateCertificates.ToArray()),
                ClientCertificate = GetCertificate(clientCert),
                Url = url,
                MaxChunkSizeInMegabytes = maxChunkSizeInMegabytes
            };

            this.meshClient = new MeshClient(meshConfigurations: this.meshConfigurations);
        }

        private static X509Certificate2Collection GetCertificates(params string[] intermediateCertificates)
        {
            var certificates = new X509Certificate2Collection();

            foreach (string item in intermediateCertificates)
            {
                certificates.Add(GetCertificate(item));
            }

            return certificates;
        }

        private static X509Certificate2 GetCertificate(string value)
        {
            byte[] certBytes = Convert.FromBase64String(value);

            return new X509Certificate2(certBytes);
        }

        private static string GetRandomString(int wordMinLength = 2, int wordMaxLength = 100) =>
            new MnemonicString(
                wordCount: 1,
                wordMinLength: 1,
                wordMaxLength: wordMaxLength < wordMinLength ? wordMinLength : wordMaxLength).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static Message CreateRandomSendMessage(
            string mexFrom,
            string mexTo,
            string mexWorkflowId,
            string mexLocalId,
            string mexSubject,
            string mexFileName,
            string mexContentChecksum,
            string mexContentEncrypted,
            string mexEncoding,
            string mexChunkRange,
            string contentType,
            string content)
        {
            var message = CreateMessageFiller(content).Create();
            message.Headers.Add("mex-from", new List<string> { mexFrom });
            message.Headers.Add("mex-to", new List<string> { mexTo });
            message.Headers.Add("mex-workflowid", new List<string> { mexWorkflowId });
            message.Headers.Add("mex-localid", new List<string> { mexLocalId });
            message.Headers.Add("mex-subject", new List<string> { mexSubject });
            message.Headers.Add("mex-filename", new List<string> { mexFileName });
            message.Headers.Add("mex-content-checksum", new List<string> { mexContentChecksum });
            message.Headers.Add("Mex-Content-Encrypted", new List<string> { mexContentEncrypted });
            message.Headers.Add("Mex-Encoding", new List<string> { mexEncoding });
            message.Headers.Add("mex-chunk-range", new List<string> { mexChunkRange });
            message.Headers.Add("content-type", new List<string> { contentType });

            return message;
        }

        private static Filler<Message> CreateMessageFiller(string content)
        {
            byte[] fileContent = Encoding.UTF8.GetBytes(content);
            var filler = new Filler<Message>();

            filler.Setup()
                .OnProperty(message => message.FileContent).Use(fileContent)
                .OnProperty(message => message.Headers).Use(new Dictionary<string, List<string>>());

            return filler;
        }
    }
}
