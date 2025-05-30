﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using NEL.MESH.Clients;
using NEL.MESH.Models.Configurations;
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
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            var url = configuration["MeshConfiguration:Url"] ?? "NULL";
            var mailboxId = configuration["MeshConfiguration:MailboxId"] ?? "NULL";
            var mexClientVersion = configuration["MeshConfiguration:MexClientVersion"] ?? "NULL";
            var mexOSName = configuration["MeshConfiguration:MexOSName"] ?? "NULL";
            var mexOSVersion = configuration["MeshConfiguration:MexOSVersion"] ?? "NULL";
            var password = configuration["MeshConfiguration:Password"] ?? "NULL";
            var sharedKey = configuration["MeshConfiguration:SharedKey"] ?? "NULL";
            var maxChunkSizeInMegabytes = int.Parse(configuration["MeshConfiguration:MaxChunkSizeInMegabytes"]);
            var clientSigningCertificate = configuration["MeshConfiguration:ClientSigningCertificate"];
            var clientSigningCertificatePassword = configuration["MeshConfiguration:ClientSigningCertificatePassword"];

            List<string> tlsRootCertificates = configuration.GetSection("MeshConfiguration:TlsRootCertificates")
                .Get<List<string>>() ?? [];

            List<string> tlsIntermediateCertificates =
                configuration.GetSection("MeshConfiguration:TlsIntermediateCertificates")
                    .Get<List<string>>() ?? [];

            this.meshConfigurations = new MeshConfiguration
            {
                Url = url,
                MailboxId = mailboxId,
                MexClientVersion = mexClientVersion,
                MexOSName = mexOSName,
                MexOSVersion = mexOSVersion,
                Password = password,
                SharedKey = sharedKey,
                TlsRootCertificates = GetCertificates(tlsRootCertificates.ToArray()),
                TlsIntermediateCertificates = GetCertificates(tlsIntermediateCertificates.ToArray()),

                ClientSigningCertificate =
                    GetPkcs12Certificate(clientSigningCertificate, clientSigningCertificatePassword),

                MaxChunkSizeInMegabytes = maxChunkSizeInMegabytes
            };

            this.meshClient = new MeshClient(meshConfigurations: this.meshConfigurations);
        }

        private static X509Certificate2Collection GetCertificates(string[] certificates)
        {
            var certificateCollection = new X509Certificate2Collection();

            foreach (string item in certificates)
            {
                certificateCollection.Add(GetPemOrDerCertificate(item));
            }

            return certificateCollection;
        }

        private static X509Certificate2 GetPemOrDerCertificate(string value)
        {
            byte[] certBytes = Convert.FromBase64String(value);
            var certificate = X509CertificateLoader.LoadCertificate(certBytes);

            return certificate;
        }

        private static X509Certificate2 GetPkcs12Certificate(string value, string password = "")
        {
            byte[] certBytes = Convert.FromBase64String(value);
            var certificate = X509CertificateLoader.LoadPkcs12(certBytes, password);

            return certificate;
        }

        private static string GetRandomString(int wordMinLength = 2, int wordMaxLength = 100) =>
            new MnemonicString(
                wordCount: 1,
                wordMinLength: 1,
                wordMaxLength: wordMaxLength < wordMinLength ? wordMinLength : wordMaxLength).GetValue();
    }
}
