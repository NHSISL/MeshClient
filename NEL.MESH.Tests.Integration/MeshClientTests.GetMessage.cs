// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.MESH.Tests.Integration
{
    public partial class MeshClientTests
    {
        //[Fact]
        //public async Task ShouldGetMessageAsync()
        //{
        //    // given
        //    string message = GetRandomString();
        //    string mailboxTo = this.meshApiConfiguration.MailboxId;
        //    string workflowId = GetRandomString();
        //    string contentType = GetRandomString();

        //    HttpResponseMessage sendMessageResponse =
        //        await this.meshBroker.SendMessageAsync(mailboxTo, workflowId, message, contentType);

        //    var sendMessageResponseBody = await sendMessageResponse.Content.ReadAsStringAsync();
        //    string messageId = (JsonConvert.DeserializeObject<SendMessageResponse>(sendMessageResponseBody)).MessageId;

        //    // when
        //    var getMessageResponse =
        //        await this.meshBroker.GetMessageAsync(messageId);

        //    var getMessageResponseBody = await getMessageResponse.Content.ReadAsStringAsync();

        //    // then
        //    sendMessageResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Accepted);
        //    getMessageResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //    message.Should().BeEquivalentTo(getMessageResponseBody);

        //    await this.meshBroker.AcknowledgeMessageAsync(messageId);
        //}
    }
}
