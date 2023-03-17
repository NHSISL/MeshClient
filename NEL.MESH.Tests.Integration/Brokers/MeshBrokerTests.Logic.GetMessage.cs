// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldGetMessageAsync()
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
            var getMessageResponse =
                await this.meshBroker.GetMessageAsync(messageId);

            var getMessageResponseBody = await getMessageResponse.Content.ReadAsStringAsync();

            // then
            sendMessageResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Accepted);
            getMessageResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            message.Should().BeEquivalentTo(getMessageResponseBody);

            await this.meshBroker.AcknowledgeMessageAsync(messageId);
        }
    }
}