// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Net.Http;
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
        [Fact]
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
                contentType);

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
            var stream = new MemoryStream(GetRandomBytes());
            var byteContent = new ByteArrayContent(stream.ToArray());
            byteContent.Headers.Add("Mex-From", mexFrom);
            byteContent.Headers.Add("Mex-To", mexTo);
            byteContent.Headers.Add("Mex-WorkflowID", mexWorkflowId);
            byteContent.Headers.Add("Mex-LocalID", mexLocalId);
            byteContent.Headers.Add("Mex-Subject", mexSubject);
            byteContent.Headers.Add("Mex-FileName", mexFileName);
            byteContent.Headers.Add("Mex-Content-Checksum", mexContentChecksum);
            byteContent.Headers.Add("Mex-Content-Encrypted", mexContentEncrypted);
            byteContent.Headers.Add("Mex-Encoding", mexEncoding);
            byteContent.Headers.Add("Mex-Chunk-Range", mexChunkRange);

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingPost()
                        .WithHeader("Mex-ClientVersion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("Mex-OSName", this.meshConfigurations.MexOSName)
                        .WithHeader("Mex-OSVersion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("Authorization", GenerateAuthorisationHeader())
                        .WithBody(byteContent))
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
