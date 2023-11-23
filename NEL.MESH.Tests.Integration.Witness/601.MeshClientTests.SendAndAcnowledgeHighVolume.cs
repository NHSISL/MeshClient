// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [Fact(DisplayName = "601 - High Volume Messages")]
        [Trait("Category", "Witness")]
        public async Task ShouldSendAndAcknowledgeHighVolumeAsync()
        {
            // given
            var messageCount = 10;

            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "WHITNESS TEST";
            string mexSubject = "WHITNESS TEST - ShouldSendMessageAsync";
            string mexFileName = $"ShouldSendMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";

            List<string> messageIds = new List<string>();

            for (int i = 0; i < messageCount; i++)
            {
                // Generate unique content for each message
                string content = GetRandomString();
                string mexLocalId = Guid.NewGuid().ToString();

                // when
                Message sendMessageResponse = await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    content,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

                // then
                sendMessageResponse.MessageId.Should().NotBeNullOrEmpty();

                // Store the MessageId for later acknowledgment
                messageIds.Add(sendMessageResponse.MessageId);
            }

            // Acknowledge each message after the loop
            foreach (string messageId in messageIds)
            {
                await this.meshClient.Mailbox.AcknowledgeMessageAsync(messageId);
            }

        }
    }
}
