// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using FluentAssertions;
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
            var password = this.configuration["MeshConfiguration:Password"];
            var key = this.configuration["MeshConfiguration:Key"];
            var rootCertificate = this.configuration["MeshConfiguration:RootCertificate"];
            var intermediateCertificates = this.configuration["MeshConfiguration:IntermediateCertificates"];
            var clientCertificate = this.configuration["MeshConfiguration:ClientCertificate"];

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            password.Should().NotBeNullOrEmpty();
            key.Should().NotBeNullOrEmpty();
            rootCertificate.Should().NotBeNullOrEmpty();
            intermediateCertificates.Should().NotBeNullOrEmpty();
            clientCertificate.Should().NotBeNullOrEmpty();
        }
    }
}
