// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.MESH.Tests.Integration
{
    public partial class MeshClientTests
    {
        //[Fact]
        //public async Task ShouldSendMessageAsync()
        //{
        //    // given
        //    string message = GetRandomString();
        //    string mailboxTo = this.meshApiConfiguration.MailboxId;
        //    string workflowId = GetRandomString();
        //    string contentType = GetRandomString();

        //    // when
        //    HttpResponseMessage sendMessageResponse =
        //        await this.meshBroker.SendMessageAsync(mailboxTo, workflowId, message, contentType);

        //    var sendMessageResponseBody = await sendMessageResponse.Content.ReadAsStringAsync();
        //    var headers = GetHeaders(sendMessageResponse.Headers);
        //    string messageId = (JsonConvert.DeserializeObject<SendMessageResponse>(sendMessageResponseBody)).MessageId;

        //    var receivedMessageResponse =
        //        await this.meshBroker.GetMessageAsync(messageId);

        //    var receivedMessageResponseBody = await receivedMessageResponse.Content.ReadAsStringAsync();

        //    // then
        //    sendMessageResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Accepted);
        //    receivedMessageResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //    receivedMessageResponseBody.Should().BeEquivalentTo(message);
        //    await this.meshBroker.AcknowledgeMessageAsync(messageId);
        //}
    }
}
