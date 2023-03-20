// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh.ExternalModeld;
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
            string contentType = GetRandomString();

            HttpResponseMessage sendMessageResponse =
                await this.meshBroker.SendMessageAsync(mailboxTo, workflowId, message, contentType);

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
    }
}