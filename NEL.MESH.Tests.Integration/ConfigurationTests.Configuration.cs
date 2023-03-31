// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
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

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            mailboxId.Should().StartWith("QM");

        }

        [Fact]
        public void ShouldGetEnvironmentVariables()
        {
            // given
            var mailboxId = Environment
                .GetEnvironmentVariable("NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__MAILBOXID");

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            mailboxId.Should().StartWith("QM");
        }

        [Fact]
        public void ShouldGetTestEnvironmentVariable()
        {
            // given
            var mailboxId = Environment.GetEnvironmentVariable("TEST");

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            mailboxId.Should().BeEquivalentTo("TEST");
        }
    }
}
