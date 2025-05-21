// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.ExternalModels;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class MeshClientTests
    {
        //[Fact]
        //[Trait("Category", "Acceptance")]
        //public async Task ShouldSendFileMessageAsync()
        //{
        //    // given
        //    string path = $"/messageexchange/{this.meshConfigurations.MailboxId}/outbox";
        //    string randomId = GetRandomString();
        //    string outputId = randomId;
        //    string mexTo = GetRandomString();
        //    string mexWorkflowId = GetRandomString();
        //    byte[] content = Encoding.UTF8.GetBytes(GetRandomString(wordMinLength: GetRandomNumber()));
        //    string mexSubject = GetRandomString();
        //    string mexLocalId = GetRandomString();
        //    string mexFileName = GetRandomString();
        //    string mexContentChecksum = GetRandomString();
        //    string contentType = "application/octet-stream";
        //    string contentEncoding = GetRandomString();

        //    Message randomMessage = ComposeMessage.CreateFileMessage(
        //        mexTo,
        //        mexWorkflowId,
        //        content,
        //        mexSubject,
        //        mexLocalId,
        //        mexFileName,
        //        mexContentChecksum,
        //        contentType,
        //        contentEncoding);

        //    Message inputMessage = randomMessage;

        //    SendMessageResponse responseMessage = new SendMessageResponse
        //    {
        //        MessageId = outputId,
        //        Message = outputId,
        //    };

        //    string serialisedResponseMessage = JsonConvert.SerializeObject(responseMessage);

        //    Message outputMessage = new Message
        //    {
        //        MessageId = outputId,
        //        FileContent = inputMessage.FileContent
        //    };

        //    Message expectedSendMessageResult = outputMessage;

        //    this.wireMockServer
        //        .Given(
        //            Request.Create()
        //                .WithPath(path)
        //                .UsingPost()
        //                .WithHeader("authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
        //                .WithHeader("mex-from", this.meshConfigurations.MailboxId)
        //                .WithHeader("mex-to", GetKeyStringValue("mex-to", inputMessage.Headers))
        //                .WithHeader("mex-workflowid", GetKeyStringValue("mex-workflowid", inputMessage.Headers))
        //                .WithHeader("mex-chunk-range", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
        //                .WithHeader("mex-subject", GetKeyStringValue("mex-subject", inputMessage.Headers))
        //                .WithHeader("mex-localid", GetKeyStringValue("mex-localid", inputMessage.Headers))
        //                .WithHeader("mex-filename", GetKeyStringValue("mex-filename", inputMessage.Headers))
        //                .WithHeader("mex-content-checksum", GetKeyStringValue("mex-content-checksum", inputMessage.Headers))
        //                .WithHeader("Accept", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
        //                .WithHeader("mex-clientversion", this.meshConfigurations.MexClientVersion)
        //                .WithHeader("mex-osname", this.meshConfigurations.MexOSName)
        //                .WithHeader("mex-osversion", this.meshConfigurations.MexOSVersion)
        //                .WithBody(randomMessage.FileContent)
        //            )
        //        .RespondWith(
        //            Response.Create()
        //                .WithSuccess()
        //                .WithBody(serialisedResponseMessage));

        //    // when
        //    Message actualSendMessageResult = await this.meshClient.Mailbox
        //        .SendMessageAsync(
        //            mexTo,
        //            mexWorkflowId,
        //            content,
        //            mexSubject,
        //            mexLocalId,
        //            mexFileName,
        //            mexContentChecksum,
        //            contentType,
        //            contentEncoding);

        //    // then
        //    actualSendMessageResult.Should().BeEquivalentTo(expectedSendMessageResult);
        //}
    }
}
