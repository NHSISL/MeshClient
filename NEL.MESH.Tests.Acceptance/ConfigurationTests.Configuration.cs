// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class ConfigurationTests
    {
        [Fact(Skip = "Excluded")]
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

            if (intermediateCertificates == null)
            {
                intermediateCertificates = new List<string>();
            }

            var clientCertificate = this.configuration["MeshConfiguration:ClientCertificate"];

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            password.Should().NotBeNullOrEmpty();
            key.Should().NotBeNullOrEmpty();
            rootCertificate.Should().NotBeNullOrEmpty();
            clientCertificate.Should().NotBeNullOrEmpty();
            intermediateCertificates.Count.Should().Be(1);
        }
    }
}
