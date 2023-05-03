﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using NEL.MESH.Clients;
using NEL.MESH.Models.Configurations;
using NEL.MESH.Models.Foundations.Mesh;
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
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("NEL_MESH_CLIENT_ACCEPTANCE_");

            IConfiguration configuration = configurationBuilder.Build();
            this.wireMockServer = WireMockServer.Start();
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

            List<string> intermediateCertificates =
                configuration.GetSection("MeshConfiguration:IntermediateCertificates")
                    .Get<List<string>>();

            if (intermediateCertificates == null)
            {
                intermediateCertificates = new List<string>();
            }

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
                Url = this.wireMockServer.Url
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

        private static string GetRandomString() =>
            new MnemonicString(wordCount: 1, wordMinLength: 1, wordMaxLength: GetRandomNumber()).GetValue();

        private static List<string> GetRandomStrings()
        {
            var randomStrings = new List<string>();
            for (int i = 0; i < GetRandomNumber(); i++)
            {
                string randomString = new MnemonicString(wordCount: 1, wordMinLength: 1, wordMaxLength: GetRandomNumber()).GetValue();
                randomStrings.Add(randomString);
            }
            return randomStrings;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static Message CreateRandomMessage()
        {
            return CreateMessageFiller(GetRandomString()).Create();
        }

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
            message.Headers.Add("Mex-From", new List<string> { mexFrom });
            message.Headers.Add("Mex-To", new List<string> { mexTo });
            message.Headers.Add("Mex-WorkflowID", new List<string> { mexWorkflowId });
            message.Headers.Add("Mex-LocalID", new List<string> { mexLocalId });
            message.Headers.Add("Mex-Subject", new List<string> { mexSubject });
            message.Headers.Add("Mex-FileName", new List<string> { mexFileName });
            message.Headers.Add("Mex-Content-Checksum", new List<string> { mexContentChecksum });
            message.Headers.Add("Mex-Content-Encrypted", new List<string> { mexContentEncrypted });
            message.Headers.Add("Mex-Encoding", new List<string> { mexEncoding });
            message.Headers.Add("Mex-Chunk-Range", new List<string> { mexChunkRange });
            message.Headers.Add("Content-Type", new List<string> { contentType });

            return message;
        }

        private static Message CreateRandomSendFile(
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
            message.Headers.Add("Mex-From", new List<string> { mexFrom });
            message.Headers.Add("Mex-To", new List<string> { mexTo });
            message.Headers.Add("Mex-WorkflowID", new List<string> { mexWorkflowId });
            message.Headers.Add("Mex-LocalID", new List<string> { mexLocalId });
            message.Headers.Add("Mex-Subject", new List<string> { mexSubject });
            message.Headers.Add("Mex-FileName", new List<string> { mexFileName });
            message.Headers.Add("Mex-Content-Checksum", new List<string> { mexContentChecksum });
            message.Headers.Add("Mex-Content-Encrypted", new List<string> { mexContentEncrypted });
            message.Headers.Add("Mex-Encoding", new List<string> { mexEncoding });
            message.Headers.Add("Mex-Chunk-Range", new List<string> { mexChunkRange });
            message.Headers.Add("Content-Type", new List<string> { contentType });

            return message;
        }

        private static Filler<Message> CreateMessageFiller(string content)
        {
            var filler = new Filler<Message>();

            filler.Setup()
                .OnProperty(message => message.StringContent).Use(content)
                .OnProperty(message => message.Headers).Use(new Dictionary<string, List<string>>());

            return filler;
        }
    }
}
