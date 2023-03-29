// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Text;
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
        public async Task ShouldSendMessageAsync()
        {
            // given
            bool expectedHandshakeResult = true;
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
            string contentType = "text/plain";

            Message randomMessage = CreateRandomSendMessage(
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

            SendMessageResponse responseMessage = new SendMessageResponse
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
            var stringContent = new StringContent("test payload", Encoding.UTF8, "application/json");
            stringContent.Headers.Add("Mex-From", mexFrom);
            stringContent.Headers.Add("Mex-To", mexTo);
            stringContent.Headers.Add("Mex-WorkflowID", mexWorkflowId);
            stringContent.Headers.Add("Mex-LocalID", mexLocalId);
            stringContent.Headers.Add("Mex-Subject", mexSubject);
            stringContent.Headers.Add("Mex-FileName", mexFileName);
            stringContent.Headers.Add("Mex-Content-Checksum", mexContentChecksum);
            stringContent.Headers.Add("Mex-Content-Encrypted", mexContentEncrypted);
            stringContent.Headers.Add("Mex-Encoding", mexEncoding);
            stringContent.Headers.Add("Mex-Chunk-Range", mexChunkRange);

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingPost()
                        .WithHeader("Mex-ClientVersion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("Mex-OSName", this.meshConfigurations.MexOSName)
                        .WithHeader("Mex-OSVersion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("Authorization", GenerateAuthorisationHeader())
                        .WithBody(stringContent))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithBody(serialisedResponseMessage));

            // when
            Message actualSendMessageResult = await this.meshClient.Mailbox
                .SendMessageAsync(randomMessage);

            // then
            actualSendMessageResult.Should().BeEquivalentTo(expectedSendMessageResult);
        }
    }
}
