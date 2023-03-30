// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class ConfigurationTests
    {
        [Fact]
        public void ShouldGetConfigurationSettings()
        {
            // given
            var mailboxId = this.configuration["MeshConfiguration:MailboxId"];
            var mexClientVersion = this.configuration["MeshConfiguration:MexClientVersion"];
            var mexOSName = this.configuration["MeshConfiguration:MexOSName"];
            var mexOSVersion = this.configuration["MeshConfiguration:MexOSVersion"];
            var password = this.configuration["MeshConfiguration:Password"];
            var key = this.configuration["MeshConfiguration:Key"];
            var clientCert = this.configuration["MeshConfiguration:ClientCertificate"];
            var rootCert = this.configuration["MeshConfiguration:RootCertificate"];

            string[] intermediateCertificates =
                this.configuration.GetSection("MeshConfiguration:IntermediateCertificates").Get<string[]>();

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            mexClientVersion.Should().NotBeNullOrEmpty();
            mexOSName.Should().NotBeNullOrEmpty();
            mexOSVersion.Should().NotBeNullOrEmpty();
            password.Should().NotBeNullOrEmpty();
            key.Should().NotBeNullOrEmpty();
            clientCert.Should().NotBeNullOrEmpty();
            rootCert.Should().NotBeNullOrEmpty();

            mailboxId.Should().StartWith("QM");
            password.Should().StartWith("0q");
            key.Should().StartWith("Ba");
            clientCert.Should().StartWith("MIITjgIBAzCCEzgGCSqGSIb3D");
            rootCert.Should().StartWith("LS0tLS1CRUdJTiBDRVJU");
        }
    }
}
