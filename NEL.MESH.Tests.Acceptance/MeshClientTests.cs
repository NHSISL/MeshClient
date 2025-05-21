﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
            var maxChunkSizeInMegabytes = int.Parse(configuration["MeshConfiguration:MaxChunkSizeInMegabytes"]);

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
                Url = this.wireMockServer.Url,
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

        private static string GetRandomString(
            int wordCount = 1,
            int wordMinLength = 1,
            int wordMaxLength = 10) =>
                new MnemonicString(
                    wordCount: wordCount,
                    wordMinLength: wordMinLength,
                    wordMaxLength: wordMaxLength == 0
                        ? GetRandomNumber()
                        : (wordMaxLength <= wordMinLength
                            ? wordMinLength + 1
                            : wordMaxLength)).GetValue();

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

        private static Filler<Message> CreateMessageFiller(string content)
        {
            byte[] fileContent = Encoding.UTF8.GetBytes(content);
            var filler = new Filler<Message>();

            filler.Setup()
                .OnProperty(message => message.FileContent).Use(fileContent)
                .OnProperty(message => message.Headers).Use(new Dictionary<string, List<string>>());

            return filler;
        }

        private static string GetKeyStringValue(string key, Dictionary<string, List<string>> dictionary)
        {
            return dictionary.ContainsKey(key)
                ? dictionary[key]?.First()
                : string.Empty;
        }
    }
}
