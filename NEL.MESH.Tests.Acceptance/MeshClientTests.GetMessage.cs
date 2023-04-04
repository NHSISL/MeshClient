// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class MeshClientTests
    {
        [Fact(Skip = "Not Ready for now")]
        public async Task ShouldGetMessageAsync()
        {
            // given
            Message randomMessage = CreateRandomMessage();
            var path = $"/messageexchange/{this.meshConfigurations.MailboxId}/inbox/{randomMessage.MessageId}";

            Message outputMessage = new Message
            {
                MessageId = randomMessage.MessageId,
                StringContent = randomMessage.StringContent
            };

            Message expectedGetMessageResult = outputMessage;

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingGet()
                        .WithHeader("Mex-ClientVersion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("Mex-OSName", this.meshConfigurations.MexOSName)
                        .WithHeader("Mex-OSVersion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("Authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
                        )
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithBody(randomMessage.StringContent));

            // randomMessage
            Message actualGetMessageResult =
                await this.meshClient.Mailbox.RetrieveMessageAsync(randomMessage.MessageId);

            // then
            actualGetMessageResult.Should().BeEquivalentTo(expectedGetMessageResult);
        }
    }
}
