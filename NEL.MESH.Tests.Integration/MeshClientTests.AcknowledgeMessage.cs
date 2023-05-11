// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            Message randomMessage = CreateRandomSendMessage(
                mexFrom: this.meshConfigurations.MailboxId,
                mexTo: this.meshConfigurations.MailboxId,
                mexWorkflowId: "INTEGRATION TEST",
                mexLocalId: GetRandomString(),
                mexSubject: "INTEGRATION TEST -  ShouldAcknowledgeMessageAsync",
                mexFileName: $"ShouldAcknowledgeMessageAsync.csv",
                mexContentChecksum: null,
                mexContentEncrypted: null,
                mexEncoding: null,
                mexChunkRange: null,
                contentType: "text/plain",
                content: GetRandomString());

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(randomMessage);

            // when
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
            List<string> messageList = await this.meshClient.Mailbox.RetrieveMessagesAsync();

            // then
            messageList.Should().NotContain(sendMessageResponse.MessageId);
        }
    }
}
