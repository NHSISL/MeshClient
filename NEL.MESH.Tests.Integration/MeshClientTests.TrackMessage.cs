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
            string mexFrom = this.meshConfigurations.MailboxId;
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "INTEGRATION TEST";
            string mexLocalId = GetRandomString();
            string mexSubject = GetRandomString();
            string mexFileName = GetRandomString();
            string mexContentChecksum = GetRandomString();
            string mexContentEncrypted = GetRandomString();
            string mexEncoding = GetRandomString();
            string mexChunkRange = GetRandomString();
            string contentType = "text/plain";

            Message randomMessage = CreateRandomSendMessage(
                mexFrom,
                mexTo,
                mexWorkflowId,
                mexLocalId,
                mexSubject,
                mexFileName,
                mexContentChecksum,
                mexContentEncrypted,
                mexEncoding,
                mexChunkRange,
                contentType);

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(randomMessage);

            // when
            Message trackedMessage = await this.meshClient.Mailbox.TrackMessageAsync(sendMessageResponse.MessageId);

            // then
            trackedMessage.MessageId.Should().BeEquivalentTo(sendMessageResponse.MessageId);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}
