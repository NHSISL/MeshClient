// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh.ExternalModeld;
using Newtonsoft.Json;
using Xunit;

namespace NEL.MESH.Tests.Integration.Brokers
{
    public partial class MeshBrokerTests
    {
        [Fact]
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            string message = GetRandomString();
            string mailboxTo = this.meshApiConfiguration.MailboxId;
            string workflowId = GetRandomString();

            HttpResponseMessage sendMessageResponse =
                await this.meshBroker.SendMessageAsync(mailboxTo, workflowId, message);

            var sendMessageResponseBody = await sendMessageResponse.Content.ReadAsStringAsync();
            string messageId = (JsonConvert.DeserializeObject<SendMessageResponse>(sendMessageResponseBody)).MessageId;

            var getMessageResponse =
                await this.meshBroker.GetMessageAsync(messageId);

            var getMessageResponseBody = await getMessageResponse.Content.ReadAsStringAsync();

            // when
            var acknowledgeMessageReposne = await this.meshBroker.AcknowledgeMessageAsync(messageId);

            // then

        }
    }
}