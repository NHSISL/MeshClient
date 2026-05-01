// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Text;
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
        public async Task ShouldRetrieveStringMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "INTEGRATION TEST";
            string content = GetRandomString();
            string mexSubject = "INTEGRATION TEST -  ShouldRetrieveStringMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldRetrieveStringMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";
            byte[] expectedBytes = Encoding.UTF8.GetBytes(content);

            using MemoryStream sendStream = new MemoryStream(expectedBytes);

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    sendStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

            using MemoryStream outputStream = new MemoryStream();

            // when
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync(sendMessageResponse.MessageId, outputStream);

            // then
            retrievedMessage.MessageId.Should().BeEquivalentTo(sendMessageResponse.MessageId);
            outputStream.ToArray().Should().BeEquivalentTo(expectedBytes);
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
            string mexSubject = "INTEGRATION TEST -  ShouldRetrieveFileMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldRetrieveFileMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "application/octet-stream";
            string contentEncoding = "";

            using MemoryStream sendStream = new MemoryStream(fileContent);

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    sendStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

            using MemoryStream outputStream = new MemoryStream();

            // when
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync(sendMessageResponse.MessageId, outputStream);

            // then
            retrievedMessage.MessageId.Should().BeEquivalentTo(sendMessageResponse.MessageId);
            outputStream.ToArray().Should().BeEquivalentTo(fileContent);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}