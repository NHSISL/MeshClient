// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Mesh;
using Newtonsoft.Json;
using Xunit;

namespace NEL.MESH.Tests.Integration.Brokers
{
    public partial class MeshBrokerTests
    {
        [Fact]
        public async Task ShouldGetMessagesAsync()
        {
            // given
            string message = GetRandomString();
            string mailboxTo = this.meshApiConfiguration.MailboxId;
            string workflowId = GetRandomString();

            HttpResponseMessage sendMessageResponse =
                await this.meshBroker.SendMessageAsync(message, mailboxTo, workflowId);

            var sendMessageResponseBody = await sendMessageResponse.Content.ReadAsStringAsync();
            string messageId = (JsonConvert.DeserializeObject<SendMessageResponse>(sendMessageResponseBody)).MessageId;

            // when
            var getMessagesResponse =
                await this.meshBroker.GetMessagesAsync();

            var getMessagesResponseBody = await getMessagesResponse.Content.ReadAsStringAsync();

            List<string> messages =
                (JsonConvert.DeserializeObject<GetMessagesResponse>(getMessagesResponseBody)).Messages;

            // then
            getMessagesResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            messages.Should().Contain(messageId);

            await this.meshBroker.AcknowledgeMessageAsync(messageId);
        }

        [Fact(Skip = "Skipped as test excluded as it will ACK the whole mailbox.")]
        //[Fact]
        public async Task ShouldGetMessagesAndTheirContentAsync()
        {
            // given
            string message = GetRandomString();
            string mailboxTo = this.meshApiConfiguration.MailboxId;
            string workflowId = GetRandomString();

            HttpResponseMessage sendMessageResponse =
                await this.meshBroker.SendMessageAsync(message, mailboxTo, workflowId);

            var sendMessageResponseBody = await sendMessageResponse.Content.ReadAsStringAsync();
            string messageId = (JsonConvert.DeserializeObject<SendMessageResponse>(sendMessageResponseBody)).MessageId;

            // when
            var getMessagesResponse =
                await this.meshBroker.GetMessagesAsync();

            var getMessagesResponseBody = await getMessagesResponse.Content.ReadAsStringAsync();

            List<string> messageIds =
                (JsonConvert.DeserializeObject<GetMessagesResponse>(getMessagesResponseBody)).Messages;

            List<string> messages = new List<string>();

            foreach (var itemId in messageIds)
            {
                string item = await (await this.meshBroker.GetMessageAsync(itemId)).Content.ReadAsStringAsync();
                messages.Add(item);
            }

            // then
            getMessagesResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            messageIds.Should().Contain(messageId);

            foreach (var itemId in messageIds)
            {
                await this.meshBroker.AcknowledgeMessageAsync(itemId);
            }
        }
    }
}