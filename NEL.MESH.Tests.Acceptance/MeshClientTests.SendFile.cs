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
        [Fact]
        [Trait("Category", "Acceptance")]
        public async Task ShouldSendFileAsync()
        {
            // given
            string path = $"/messageexchange/{this.meshConfigurations.MailboxId}/outbox";
            string randomId = GetRandomString();
            string outputId = randomId;
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

            Message inputMessage = randomMessage;

            SendMessageResponse responseMessage = new SendMessageResponse
            {
                MessageId = outputId,
                Message = outputId,
            };

            string serialisedResponseMessage = JsonConvert.SerializeObject(responseMessage);

            Message outputMessage = new Message
            {
                MessageId = outputId,
                FileContent = inputMessage.FileContent
            };

            Message expectedSendMessageResult = outputMessage;

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingPost()
                        .WithHeader("Mex-ClientVersion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("Mex-OSName", this.meshConfigurations.MexOSName)
                        .WithHeader("Mex-OSVersion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("Mex-From", this.meshConfigurations.MailboxId)
                        .WithHeader("Mex-To", GetKeyStringValue("Mex-To", inputMessage.Headers))
                        .WithHeader("Mex-WorkflowID", GetKeyStringValue("Mex-WorkflowID", inputMessage.Headers))
                        .WithHeader("Mex-LocalID", GetKeyStringValue("Mex-LocalID", inputMessage.Headers))
                        .WithHeader("Mex-Subject", GetKeyStringValue("Mex-Subject", inputMessage.Headers))
                        .WithHeader("Mex-FileName", GetKeyStringValue("Mex-FileName", inputMessage.Headers))
                        .WithHeader("Mex-Content-Checksum", GetKeyStringValue("Mex-Content-Checksum", inputMessage.Headers))
                        .WithHeader("Mex-Content-Encrypted", GetKeyStringValue("Mex-Content-Encrypted", inputMessage.Headers))
                        .WithHeader("Mex-Encoding", GetKeyStringValue("Mex-Encoding", inputMessage.Headers))
                        .WithHeader("Mex-Chunk-Range", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
                        .WithHeader("Authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
                        .WithBody(randomMessage.FileContent))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithBody(serialisedResponseMessage));

            // when
            Message actualSendMessageResult = await this.meshClient.Mailbox
                .SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding
                );

            // then
            actualSendMessageResult.Should().BeEquivalentTo(expectedSendMessageResult);
        }
    }
}
