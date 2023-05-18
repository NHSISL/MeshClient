// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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

            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "INTEGRATION TEST";
            string content = GetRandomString();
            string mexSubject = "INTEGRATION TEST -  ShouldTrackMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldTrackMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    content,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

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
