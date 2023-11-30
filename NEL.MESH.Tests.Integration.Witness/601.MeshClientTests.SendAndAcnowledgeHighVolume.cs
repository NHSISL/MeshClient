// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [Theory(DisplayName = "601 - High Volume Messages")]
        [InlineData(6)]
        [Trait("Category", "Witness")]
        public async Task ShouldSenddAndAcknowledgeHighVolumeAsync(int value)
        {
            // given
            var messageCount = value;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "WHITNESS TEST";
            string mexSubject = "WHITNESS TEST - ShouldSendMessageAsync";
            string mexFileName = $"ShouldSendMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";

            // when
            for (int i = 0; i < messageCount; i++)
            {
                string content = GetRandomString();
                string mexLocalId = Guid.NewGuid().ToString();

                Models.Foundations.Mesh.Message sendMessageResponse = await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    content,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);
            }

            stopwatch.Stop();
            output.WriteLine($"Total Time taken to send:{stopwatch.ElapsedMilliseconds} milliseconds");
            output.WriteLine($"Time taken per message to send:{stopwatch.ElapsedMilliseconds / messageCount} milliseconds");
            output.WriteLine("");

            // then

            List<string> messageIds;
            Stopwatch readStopwatch = new Stopwatch();
            readStopwatch.Start();

            while ((messageIds = await this.meshClient.Mailbox.RetrieveMessagesAsync()).Count > 0)
            {
                output.WriteLine($"Total messages:{messageIds.Count}");

                foreach (var item in messageIds)
                {
                    await this.meshClient.Mailbox.RetrieveMessageAsync(item);
                    await this.meshClient.Mailbox.AcknowledgeMessageAsync(item);
                }

                messageIds = await this.meshClient.Mailbox.RetrieveMessagesAsync();
                output.WriteLine($"Total messages (second Attempt:{messageIds.Count}");
            }

            readStopwatch.Stop();
            output.WriteLine($"Total Time taken to read:{readStopwatch.ElapsedMilliseconds} milliseconds");
            output.WriteLine($"Time taken per message to read:{readStopwatch.ElapsedMilliseconds / messageCount} milliseconds");
        }

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
                Models.Foundations.Mesh.Message sendMessageResponse = await this.meshClient.Mailbox.SendMessageAsync(
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
