// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class MeshClientTests
    {
        [Fact]
        [Trait("Category", "Acceptance")]
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
                        .WithHeader("Mex-ClientVersion", "*", MatchBehaviour.AcceptOnMatch)
                        .WithHeader("Mex-OSName", "*", MatchBehaviour.AcceptOnMatch)
                        .WithHeader("Mex-OSVersion", "*", MatchBehaviour.AcceptOnMatch))
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
