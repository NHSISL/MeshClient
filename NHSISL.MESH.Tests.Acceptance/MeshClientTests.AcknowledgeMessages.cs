// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NHSISL.MESH.Tests.Acceptance
{
    public partial class MeshClientTests
    {
        [Fact]
        [Trait("Category", "Acceptance")]
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            string messageId = GetRandomString();
            var path =
                $"/messageexchange/{this.meshConfigurations.MailboxId}/inbox/{messageId}/status/acknowledged";

            bool expectedAcknowledgeMessageResult = true;

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingPut()
                        .WithHeader("mex-clientversion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("mex-osname", this.meshConfigurations.MexOSName)
                        .WithHeader("mex-osversion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("authorization", "*", MatchBehaviour.AcceptOnMatch)
                        )
                .RespondWith(
                    Response.Create()
                        .WithSuccess());

            // when
            bool actualGetMessageResult =
                await this.meshClient.Mailbox.AcknowledgeMessageAsync(messageId);

            // then
            actualGetMessageResult.Should().Be(expectedAcknowledgeMessageResult);
        }
    }
}
