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
            string path = $"/messageexchange/{this.meshConfigurations.MailboxId}";

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingGet()
                        .WithHeader("Mex-ClientVersion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("Mex-OSName", this.meshConfigurations.MexOSName)
                        .WithHeader("Mex-OSVersion", this.meshConfigurations.MexOSVersion))
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
