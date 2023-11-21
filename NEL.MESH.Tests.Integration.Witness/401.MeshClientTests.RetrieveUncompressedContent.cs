// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [Fact(DisplayName = "401 - Retrieve Uncompressed Content")]
        [Trait("Category", "Witness")]
        public async Task ShouldRetrieveStringMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "401 WITNESS TEST";
            string content = "9694116538, 9694116414"; //Test Patients
            string mexSubject = "WITNESS TEST -  ShouldRetrieveStringMessageUncompressed";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldRetrieveStringMessageUncompressed.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";

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
            retrievedMessage.FileContent.Should().BeEquivalentTo(expectedMessage.FileContent);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }

        [Fact]
        [Trait("Category", "Witness")]
        public async Task ShouldRetrieveFileMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "401 WITNESS TEST FILE";
            byte[] fileContent = Encoding.ASCII.GetBytes("9694116538, 9694116414");
            string mexSubject = "WITNESS TEST -  ShouldRetrieveFileMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldRetrieveFileMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "application/octet-stream";
            string contentEncoding = "";

            Message randomMessage = ComposeMessage.CreateFileMessage(
                mexTo,
                mexWorkflowId,
                fileContent,
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
            retrievedMessage.FileContent.Should().BeEquivalentTo(expectedMessage.FileContent);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}
