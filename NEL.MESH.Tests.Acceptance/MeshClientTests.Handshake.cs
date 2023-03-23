// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class MeshClientTests
    {
        [Fact]
        public async Task ShouldDoHandshakeAsync()
        {
            // given
            bool expectedHandshakeResult = true;

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath($"/messageexchange/{this.meshConfigurations.MailboxId}/outbox")
                        .WithHeader("Mex-ClientVersion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("Mex-OSName", this.meshConfigurations.MexOSName)
                        .WithHeader("Mex-OSVersion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("Authorization", GenerateAuthorisationHeader()))
                .RespondWith(
                    Response.Create()
                        .WithSuccess());

            // when
            bool actualHandshakeResult = await this.meshClient.Mailbox.HandshakeAsync();

            // then
            actualHandshakeResult.Should().Be(expectedHandshakeResult);
        }
    }
}
