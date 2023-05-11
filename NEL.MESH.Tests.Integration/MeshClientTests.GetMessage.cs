// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration
{
    public partial class MeshClientTests
    {
        [Fact(Skip = "Excluded")]
        [Trait("Category", "Integration")]
        public async Task ShouldGetMessageAsync()
        {
            // given
            Message randomMessage = CreateRandomSendMessage(
                mexFrom: this.meshConfigurations.MailboxId,
                mexTo: this.meshConfigurations.MailboxId,
                mexWorkflowId: "INTEGRATION TEST",
                mexLocalId: GetRandomString(),
                mexSubject: "INTEGRATION TEST -  ShouldGetMessageAsync",
                mexFileName: $"ShouldGetMessageAsync.csv",
                mexContentChecksum: null,
                mexContentEncrypted: null,
                mexEncoding: null,
                mexChunkRange: null,
                contentType: "text/plain",
                content: GetRandomString());

            Message expectedMessage = randomMessage;

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(randomMessage);

            // when
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync(sendMessageResponse.MessageId);

            // then
            retrievedMessage.MessageId.Should().BeEquivalentTo(sendMessageResponse.MessageId);
            retrievedMessage.StringContent.Should().BeEquivalentTo(expectedMessage.StringContent);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}
