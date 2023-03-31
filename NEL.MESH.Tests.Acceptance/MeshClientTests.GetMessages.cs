// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class MeshClientTests
    {
        [Fact]
        public async Task ShouldGetMessagesAsync()
        {
            // given
            string path = $"/messageexchange/{this.meshConfigurations.MailboxId}";

            List<string> randomMessages = GetRandomStrings();

            string serialisedResponseMessage = JsonConvert.SerializeObject(randomMessages);

            List<string> expectedGetMessagesResult = randomMessages;

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
                        .WithBody(serialisedResponseMessage));

            // when
            List<string> actualGetMessagesResult = await this.meshClient.Mailbox.RetrieveMessagesAsync();

            // then
            actualGetMessagesResult.Should().BeEquivalentTo(expectedGetMessagesResult);
        }
    }
}
