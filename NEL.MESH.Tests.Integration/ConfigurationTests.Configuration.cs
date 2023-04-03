// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace NEL.MESH.Tests.Integration
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
            var intermediates = this.configuration["MeshConfiguration:IntermediateCertificates"];
            List<string> intermediateCertificates = JsonConvert.DeserializeObject<List<string>>(intermediates);
            var clientCertificate = this.configuration["MeshConfiguration:ClientCertificate"];

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            password.Should().NotBeNullOrEmpty();
            key.Should().NotBeNullOrEmpty();
            rootCertificate.Should().NotBeNullOrEmpty();
            Assert.True(intermediates.Length > 0, intermediates);
            intermediates.Should().NotBeNullOrEmpty();
            intermediateCertificates.Count().Should().BeGreaterThan(0);
            clientCertificate.Should().NotBeNullOrEmpty();
        }
    }
}
