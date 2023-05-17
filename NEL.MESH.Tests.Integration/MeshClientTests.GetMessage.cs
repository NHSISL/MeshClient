// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration
{
    public partial class MeshClientTests
    {
        [Fact(Skip = "Waiting on reply from nhs digital")]
        [Trait("Category", "Integration")]
        public async Task ShouldRetrieveStringMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "INTEGRATION TEST";
            string content = GetRandomString();
            string mexSubject = "INTEGRATION TEST -  ShouldRetrieveStringMessageAsync";
            string mexLocalId = GetRandomString();
            string mexFileName = $"ShouldRetrieveStringMessageAsync.csv";
            string mexContentChecksum = GetRandomString();
            string contentType = "text/plain";
            string contentEncoding = GetRandomString();

            Message randomMessage = ComposeMessage.CreateStringMessage(
                mexTo,
                mexWorkflowId,
                content,
                mexSubject,
                mexLocalId,
                mexFileName,
                mexContentChecksum,
                contentType,
                contentEncoding);

            Message expectedMessage = randomMessage;

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
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync(sendMessageResponse.MessageId);

            // then
            retrievedMessage.MessageId.Should().BeEquivalentTo(sendMessageResponse.MessageId);
            retrievedMessage.StringContent.Should().BeEquivalentTo(expectedMessage.StringContent);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldRetrieveFileMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "INTEGRATION TEST";
            byte[] fileContent = Encoding.ASCII.GetBytes(GetRandomString());
            string mexContentEncrypted = GetRandomString();
            string mexSubject = "INTEGRATION TEST -  ShouldRetrieveFileMessageAsync";
            string mexLocalId = GetRandomString();
            string mexFileName = $"ShouldRetrieveFileMessageAsync.csv";
            string mexContentChecksum = GetRandomString();
            string contentType = "application/octet-stream";
            string contentEncoding = GetRandomString();

            Message randomMessage = ComposeMessage.CreateFileMessage(
                mexTo,
                mexWorkflowId,
                fileContent,
                mexContentEncrypted,
                mexSubject,
                mexLocalId,
                mexFileName,
                mexContentChecksum,
                contentType,
                contentEncoding);

            Message expectedMessage = randomMessage;

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexContentEncrypted,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

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
