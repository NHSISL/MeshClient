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
        public async Task ShouldAcknowledgeMessageAsync()
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
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync(sendMessageResponse.MessageId);

            // then
            retrievedMessage.MessageId.Should().BeEquivalentTo(sendMessageResponse.MessageId);
            retrievedMessage.StringContent.Should().BeEquivalentTo(sendMessageResponse.StringContent);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);

        }
    }
}
