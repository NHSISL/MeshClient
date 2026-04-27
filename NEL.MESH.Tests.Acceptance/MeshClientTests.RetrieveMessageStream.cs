// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
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
        [Fact]
        [Trait("Category", "Acceptance")]
        public async Task ShouldGetStringMessageToStreamAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            string mexTo = GetRandomString();
            string mexWorkflowId = GetRandomString();
            string content = GetRandomString(wordMinLength: GetRandomNumber());
            string mexSubject = GetRandomString();
            string mexLocalId = GetRandomString();
            string mexFileName = GetRandomString();
            string mexContentChecksum = GetRandomString();
            string contentType = "text/plain";
            string contentEncoding = GetRandomString();

            Message randomMessage = ComposeMessage.CreateStringMessage(
                mexTo,
                mexWorkflowId,
                content,
                mexSubject,
                mexLocalId,
                mexFileName,
                mexContentChecksum,
                contentType,
                contentEncoding);

            randomMessage.MessageId = inputMessageId;
            byte[] expectedBytes = randomMessage.FileContent;
            var path = $"/messageexchange/{this.meshConfigurations.MailboxId}/inbox/{inputMessageId}";

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingGet()
                        .WithHeader("mex-clientversion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("mex-osname", this.meshConfigurations.MexOSName)
                        .WithHeader("mex-osversion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithHeader("content-type", contentType)
                        .WithBody(randomMessage.FileContent));

            using MemoryStream outputStream = new MemoryStream();

            // when
            Message actualGetMessageResult =
                await this.meshClient.Mailbox.RetrieveMessageAsync(inputMessageId, outputStream);

            // then
            actualGetMessageResult.MessageId.Should().BeEquivalentTo(inputMessageId);
            actualGetMessageResult.FileContent.Should().BeNull();
            outputStream.ToArray().Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Acceptance")]
        public async Task ShouldGetFileMessageToStreamAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            string mexTo = GetRandomString();
            string mexWorkflowId = GetRandomString();
            byte[] fileContent = Encoding.ASCII.GetBytes(GetRandomString(wordMinLength: GetRandomNumber()));
            string mexContentEncrypted = GetRandomString();
            string mexSubject = GetRandomString();
            string mexLocalId = GetRandomString();
            string mexFileName = GetRandomString();
            string mexContentChecksum = GetRandomString();
            string contentType = "application/octet-stream";
            string contentEncoding = GetRandomString();

            Message randomMessage = ComposeMessage.CreateFileMessage(
                mexTo,
                mexWorkflowId,
                fileContent,
                mexContentEncrypted,
                mexSubject,
                mexLocalId,
                mexFileName,
                mexContentChecksum,
                contentType,
                contentEncoding);

            randomMessage.MessageId = inputMessageId;
            byte[] expectedBytes = randomMessage.FileContent;
            var path = $"/messageexchange/{this.meshConfigurations.MailboxId}/inbox/{inputMessageId}";

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingGet()
                        .WithHeader("mex-clientversion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("mex-osname", this.meshConfigurations.MexOSName)
                        .WithHeader("mex-osversion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithHeader("content-type", contentType)
                        .WithBody(randomMessage.FileContent));

            using MemoryStream outputStream = new MemoryStream();

            // when
            Message actualGetMessageResult =
                await this.meshClient.Mailbox.RetrieveMessageAsync(inputMessageId, outputStream);

            // then
            actualGetMessageResult.MessageId.Should().BeEquivalentTo(inputMessageId);
            actualGetMessageResult.FileContent.Should().BeNull();
            outputStream.ToArray().Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Acceptance")]
        public async Task ShouldGetChunkedFileMessageToStreamAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            string mexTo = GetRandomString();
            string mexWorkflowId = GetRandomString();
            byte[] chunk1Bytes = Encoding.ASCII.GetBytes(GetRandomString(wordMinLength: GetRandomNumber()));
            byte[] chunk2Bytes = Encoding.ASCII.GetBytes(GetRandomString(wordMinLength: GetRandomNumber()));
            string contentType = "application/octet-stream";
            var path1 = $"/messageexchange/{this.meshConfigurations.MailboxId}/inbox/{inputMessageId}";
            var path2 = $"/messageexchange/{this.meshConfigurations.MailboxId}/inbox/{inputMessageId}/2";

            byte[] expectedBytes = new byte[chunk1Bytes.Length + chunk2Bytes.Length];
            chunk1Bytes.CopyTo(expectedBytes, 0);
            chunk2Bytes.CopyTo(expectedBytes, chunk1Bytes.Length);

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path1)
                        .UsingGet()
                        .WithHeader("mex-clientversion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("mex-osname", this.meshConfigurations.MexOSName)
                        .WithHeader("mex-osversion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithHeader("content-type", contentType)
                        .WithHeader("mex-chunk-range", "{1:2}")
                        .WithBody(chunk1Bytes));

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path2)
                        .UsingGet()
                        .WithHeader("mex-clientversion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("mex-osname", this.meshConfigurations.MexOSName)
                        .WithHeader("mex-osversion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithHeader("content-type", contentType)
                        .WithBody(chunk2Bytes));

            using MemoryStream outputStream = new MemoryStream();

            // when
            Message actualGetMessageResult =
                await this.meshClient.Mailbox.RetrieveMessageAsync(inputMessageId, outputStream);

            // then
            actualGetMessageResult.MessageId.Should().BeEquivalentTo(inputMessageId);
            actualGetMessageResult.FileContent.Should().BeNull();
            outputStream.ToArray().Should().BeEquivalentTo(expectedBytes);
        }
    }
}
