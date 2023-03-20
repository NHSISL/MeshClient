// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.MESH.Tests.Integration.Brokers
{
    public partial class MeshBrokerTests
    {
        //[Fact]
        //public async Task ShouldTrackMessageAsync()
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
        //    var trackMessageResponse =
        //        await this.meshBroker.TrackMessageAsync(messageId);

        //    var trackMessageResponseBody = await trackMessageResponse.Content.ReadAsStringAsync();

        //    // then
        //    sendMessageResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Accepted);
        //    trackMessageResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //    await this.meshBroker.GetMessageAsync(messageId);
        //    await this.meshBroker.AcknowledgeMessageAsync(messageId);
        //}
    }
}