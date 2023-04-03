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
        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldTrackMessageAsync()
        {
            // given
            string sender = this.meshConfigurations.MailboxId;
            string recipient = this.meshConfigurations.MailboxId;
            string workflowId = "INTEGRATION TEST";

            Message randomMessage = CreateRandomSendMessage(
                mexFrom: sender,
                mexTo: recipient,
                mexWorkflowId: workflowId,
                mexLocalId: GetRandomString(),
                mexSubject: "INTEGRATION TEST -  ShouldTrackMessageAsync",
                mexFileName: $"ShouldTrackMessageAsync.csv",
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
            Message trackedMessage = await this.meshClient.Mailbox.TrackMessageAsync(sendMessageResponse.MessageId);

            // then
            trackedMessage.MessageId.Should().BeEquivalentTo(sendMessageResponse.MessageId);
            trackedMessage.TrackingInfo.Should().NotBeNull();
            trackedMessage.TrackingInfo.Sender.Should().BeEquivalentTo(sender);
            trackedMessage.TrackingInfo.Recipient.Should().BeEquivalentTo(recipient);
            trackedMessage.TrackingInfo.WorkflowId.Should().BeEquivalentTo(workflowId);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}
