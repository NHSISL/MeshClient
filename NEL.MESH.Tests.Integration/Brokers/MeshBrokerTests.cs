// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Models.Configurations;
using Tynamix.ObjectFiller;

namespace NEL.MESH.Tests.Integration.Brokers
{
    public partial class MeshBrokerTests
    {
        private readonly MeshConfigurations meshConfiguration;
        private readonly IMeshBroker meshBroker;

        public MeshBrokerTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = configurationBuilder.Build();

            string base64RootCertificate = configuration["MeshConfiguration:RootCertificate"];
            X509Certificate2 rootCertificate = CreateCertificate(base64RootCertificate);
            X509Certificate2Collection intermediateCertificates = new X509Certificate2Collection();

            string[] intermediateCerts =
                configuration.GetSection("MeshConfiguration:IntermediateCertificates")
                .GetChildren()
                .Select(item => item.Value.ToString())
                .ToArray();

            foreach (var base64Cert in intermediateCerts)
            {
                X509Certificate2 cert = CreateCertificate(base64RootCertificate);
                intermediateCertificates.Add(cert);
            }

            string base64ClientCertificate = configuration["MeshConfiguration:ClientCertificate"];
            string clientCertificatePassword = configuration["MeshConfiguration:ClientCertificatePassword"];
            X509Certificate2 clientCertificate = CreateCertificate(base64ClientCertificate, clientCertificatePassword);

            this.meshConfiguration = new MeshConfigurations()
            {
                MailboxId = configuration["MeshConfiguration:MailboxId"],
                Password = configuration["MeshConfiguration:Password"],
                Url = configuration["MeshConfiguration:Url"],
                RootCertificate = rootCertificate,
                IntermediateCertificates = intermediateCertificates,
                ClientCertificate = clientCertificate,
                MexClientVersion = configuration["MeshConfiguration:MexClientVersion"],
                MexOSName = configuration["MeshConfiguration:MexOSName"],
                MexOSVersion = configuration["MeshConfiguration:MexOSVersion"],
            };

            this.meshBroker = new MeshBroker(this.meshConfiguration);
        }

        private static X509Certificate2 CreateCertificate(string base64RootCertificate, string? password = null)
        {
            if (string.IsNullOrEmpty(password) == true)
            {
                return new X509Certificate2(Convert.FromBase64String(base64RootCertificate));
            }

            return new X509Certificate2(Convert.FromBase64String(base64RootCertificate), password);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private Dictionary<string, List<string>> GetHeaders(HttpResponseHeaders headers)
        {
            var dictionary = new Dictionary<string, List<string>>();

            foreach (var header in headers)
            {
                dictionary.Add(header.Key, header.Value.ToList());
            }

            return dictionary;
        }
    }
}