﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace NHSISL.MESH.Tests.Integration.Witness
{
    public partial class ConfigurationTests
    {
        [WitnessTestsFact]
        public void ShouldGetConfigurationSettings()
        {
            // given
            var mailboxId = this.configuration["MeshConfiguration:MailboxId"];
            var password = this.configuration["MeshConfiguration:Password"];
            var key = this.configuration["MeshConfiguration:Key"];
            var rootCertificate = this.configuration["MeshConfiguration:RootCertificate"];

            var intermediateCertificates =
                this.configuration.GetSection("MeshConfiguration:IntermediateCertificates")
                    .Get<List<string>>();

            var clientCertificate = this.configuration["MeshConfiguration:ClientCertificate"];

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            password.Should().NotBeNullOrEmpty();
            key.Should().NotBeNullOrEmpty();
            rootCertificate.Should().NotBeNullOrEmpty();
            intermediateCertificates.Count().Should().BeGreaterThan(0);
            clientCertificate.Should().NotBeNullOrEmpty();
            intermediateCertificates.Count.Should().Be(1);
        }
    }
}
