// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Foundations.Mesh;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class MeshClientTests
    {
        //[Fact]
        //[Trait("Category", "Acceptance")]
        //public async Task ShouldGetStringMessageAsync()
        //{
        //    // given
        //    string randomMessageId = GetRandomString();
        //    string inputMessageId = randomMessageId;
        //    string mexTo = GetRandomString();
        //    string mexWorkflowId = GetRandomString();
        //    string content = GetRandomString(wordMinLength: GetRandomNumber());
        //    string mexSubject = GetRandomString();
        //    string mexLocalId = GetRandomString();
        //    string mexFileName = GetRandomString();
        //    string mexContentChecksum = GetRandomString();
        //    string contentType = "text/plain";
        //    string contentEncoding = GetRandomString();


        //    Message randomMessage = ComposeMessage.CreateStringMessage(
        //        mexTo,
        //        mexWorkflowId,
        //        content,
        //        mexSubject,
        //        mexLocalId,
        //        mexFileName,
        //        mexContentChecksum,
        //        contentType,
        //        contentEncoding);

        //    randomMessage.MessageId = inputMessageId;

        //    var path = $"/messageexchange/{this.meshConfigurations.MailboxId}/inbox/{inputMessageId}";

        //    Message outputMessage = new Message
        //    {
        //        MessageId = randomMessage.MessageId,
        //        FileContent = randomMessage.FileContent
        //    };

        //    Message expectedGetMessageResult = outputMessage;

        //    this.wireMockServer
        //        .Given(
        //            Request.Create()
        //                .WithPath(path)
        //                .UsingGet()
        //                .WithHeader("mex-clientversion", this.meshConfigurations.MexClientVersion)
        //                .WithHeader("mex-osname", this.meshConfigurations.MexOSName)
        //                .WithHeader("mex-osversion", this.meshConfigurations.MexOSVersion)
        //                .WithHeader("authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
        //                )
        //        .RespondWith(
        //            Response.Create()
        //                .WithSuccess()
        //                .WithHeader("content-type", contentType)
        //                .WithBody(randomMessage.FileContent));

        //    // when
        //    Message actualGetMessageResult =
        //        await this.meshClient.Mailbox.RetrieveMessageAsync(inputMessageId);

        //    // then
        //    actualGetMessageResult.MessageId.Should().BeEquivalentTo(expectedGetMessageResult.MessageId);
        //    actualGetMessageResult.FileContent.Should().BeEquivalentTo(expectedGetMessageResult.FileContent);
        //}

        //[Fact]
        //[Trait("Category", "Acceptance")]
        //public async Task ShouldGetFileMessageAsync()
        //{
        //    // given
        //    string randomMessageId = GetRandomString();
        //    string inputMessageId = randomMessageId;
        //    string mexTo = GetRandomString();
        //    string mexWorkflowId = GetRandomString();
        //    byte[] fileContent = Encoding.ASCII.GetBytes(GetRandomString(wordMinLength: GetRandomNumber()));
        //    string mexContentEncrypted = GetRandomString();
        //    string mexSubject = GetRandomString();
        //    string mexLocalId = GetRandomString();
        //    string mexFileName = GetRandomString();
        //    string mexContentChecksum = GetRandomString();
        //    string contentType = "application/octet-stream";
        //    string contentEncoding = GetRandomString();

        //    Message randomMessage = ComposeMessage.CreateFileMessage(
        //        mexTo,
        //        mexWorkflowId,
        //        fileContent,
        //        mexContentEncrypted,
        //        mexSubject,
        //        mexLocalId,
        //        mexFileName,
        //        mexContentChecksum,
        //        contentType,
        //        contentEncoding);

        //    randomMessage.MessageId = inputMessageId;

        //    var path = $"/messageexchange/{this.meshConfigurations.MailboxId}/inbox/{inputMessageId}";

        //    Message outputMessage = new Message
        //    {
        //        MessageId = randomMessage.MessageId,
        //        FileContent = randomMessage.FileContent
        //    };

        //    Message expectedGetMessageResult = outputMessage;

        //    this.wireMockServer
        //        .Given(
        //            Request.Create()
        //                .WithPath(path)
        //                .UsingGet()
        //                .WithHeader("mex-clientversion", this.meshConfigurations.MexClientVersion)
        //                .WithHeader("mex-osname", this.meshConfigurations.MexOSName)
        //                .WithHeader("mex-osversion", this.meshConfigurations.MexOSVersion)
        //                .WithHeader("authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
        //                )
        //        .RespondWith(
        //            Response.Create()
        //                .WithSuccess()
        //                .WithHeader("content-type", contentType)
        //                .WithBody(randomMessage.FileContent));

        //    // when
        //    Message actualGetMessageResult =
        //        await this.meshClient.Mailbox.RetrieveMessageAsync(inputMessageId);

        //    // then
        //    actualGetMessageResult.MessageId.Should().BeEquivalentTo(expectedGetMessageResult.MessageId);
        //    actualGetMessageResult.FileContent.Should().BeEquivalentTo(expectedGetMessageResult.FileContent);
        //}
    }
}
