﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace NEL.MESH.Tests.Integration
{
    public partial class ConfigurationTests
    {
        [Fact]
        public void ShouldGetConfigurationSettings()
        {
            // given
            var url = configuration["MeshConfiguration:Url"];
            var mailboxId = this.configuration["MeshConfiguration:MailboxId"];
            var password = this.configuration["MeshConfiguration:Password"];
            var sharedKey = this.configuration["MeshConfiguration:SharedKey"];
            var clientSigningCertificate = this.configuration["MeshConfiguration:ClientSigningCertificate"];

            var tlsRootCertificates = this.configuration.GetSection("MeshConfiguration:TlsRootCertificates")
                .Get<List<string>>();

            var tlsIntermediateCertificates =
                this.configuration.GetSection("MeshConfiguration:TlsIntermediateCertificates")
                    .Get<List<string>>();

            // then
            url.Should().NotBeNullOrEmpty();
            mailboxId.Should().NotBeNullOrEmpty();
            password.Should().NotBeNullOrEmpty();
            sharedKey.Should().NotBeNullOrEmpty();
            tlsRootCertificates.Count.Should().Be(1);
            tlsIntermediateCertificates.Count().Should().BeGreaterThan(0);
            clientSigningCertificate.Should().NotBeNullOrEmpty();
            tlsIntermediateCertificates.Count.Should().Be(1);
        }
    }
}
