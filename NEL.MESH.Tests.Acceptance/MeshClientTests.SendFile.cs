// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
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
        [Fact(Skip = "Not Ready for now")]
        public async Task ShouldSendFileAsync()
        {
            // given
            string path = $"/messageexchange/{this.meshConfigurations.MailboxId}/outbox";
            string mexFrom = GetRandomString();
            string mexTo = GetRandomString();
            string mexWorkflowId = GetRandomString();
            string mexLocalId = GetRandomString();
            string mexSubject = GetRandomString();
            string mexFileName = GetRandomString();
            string mexContentChecksum = GetRandomString();
            string mexContentEncrypted = GetRandomString();
            string mexEncoding = GetRandomString();
            string mexChunkRange = GetRandomString();
            string contentType = "application/octet-stream";
            string content = GetRandomString();

            Message randomMessage = CreateRandomSendFile(
                mexFrom,
                mexTo,
                mexWorkflowId,
                mexLocalId,
                mexSubject,
                mexFileName,
                mexContentChecksum,
                mexContentEncrypted,
                mexEncoding,
                mexChunkRange,
                contentType,
                content);

            SendFileResponse responseMessage = new SendFileResponse
            {
                MessageId = randomMessage.MessageId,
                Message = randomMessage.MessageId
            };

            string serialisedResponseMessage = JsonConvert.SerializeObject(responseMessage);

            Message outputMessage = new Message
            {
                MessageId = randomMessage.MessageId,
                StringContent = serialisedResponseMessage
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
                        .WithHeader("Mex-To", mexTo)
                        .WithHeader("Mex-WorkflowID", mexWorkflowId)
                        .WithHeader("Mex-LocalID", mexLocalId)
                        .WithHeader("Mex-Subject", mexSubject)
                        .WithHeader("Mex-FileName", mexFileName)
                        .WithHeader("Mex-Content-Checksum", mexContentChecksum)
                        .WithHeader("Mex-Content-Encrypted", mexContentEncrypted)
                        .WithHeader("Mex-Encoding", mexEncoding)
                        .WithHeader("Mex-Chunk-Range", mexChunkRange)
                        .WithHeader("Authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
                        .WithBody(randomMessage.FileContent))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithBody(serialisedResponseMessage));

            // when
            Message actualSendMessageResult = await this.meshClient.Mailbox
                .SendFileAsync(randomMessage);

            // then
            actualSendMessageResult.Should().BeEquivalentTo(expectedSendMessageResult);
        }
    }
}
