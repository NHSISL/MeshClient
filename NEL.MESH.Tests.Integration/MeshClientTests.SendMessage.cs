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
        public async Task ShouldSendMessageAsync()
        {
            // given
            Message randomMessage = CreateRandomSendMessage(
                mexFrom: this.meshConfigurations.MailboxId,
                mexTo: this.meshConfigurations.MailboxId,
                mexWorkflowId: "INTEGRATION TEST",
                mexLocalId: GetRandomString(),
                mexSubject: "INTEGRATION TEST -  ShouldSendMessageAsync",
                mexFileName: $"ShouldSendMessageAsync.csv",
                mexContentChecksum: null,
                mexContentEncrypted: null,
                mexEncoding: null,
                mexChunkRange: null,
                contentType: "text/plain",
                content: GetRandomString());

            // when
            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(randomMessage);

            // then
            sendMessageResponse.MessageId.Should().NotBeNullOrEmpty();
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}
