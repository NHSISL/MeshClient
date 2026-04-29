// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
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
        [Trait("Category", "Acceptance")]
        public async Task ShouldSendStringMessageAsync()
        {
            // given
            string path = $"/messageexchange/{this.meshConfigurations.MailboxId}/outbox";
            string randomId = GetRandomString();
            string outputId = randomId;
            string mexTo = GetRandomString();
            string mexWorkflowId = GetRandomString();
            string content = GetRandomString(wordMinLength: GetRandomNumber());
            string mexSubject = GetRandomString();
            string mexLocalId = GetRandomString();
            string mexFileName = GetRandomString();
            string mexContentChecksum = GetRandomString();
            string contentType = "text/plain";
            string contentEncoding = GetRandomString();
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);

            SendMessageResponse responseMessage = new SendMessageResponse
            {
                MessageId = outputId,
                Message = outputId,
            };

            string serialisedResponseMessage = JsonConvert.SerializeObject(responseMessage);
            Message expectedSendMessageResult = new Message { MessageId = outputId };

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingPost()
                        .WithHeader("authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
                        .WithHeader("mex-from", this.meshConfigurations.MailboxId)
                        .WithHeader("mex-to", mexTo)
                        .WithHeader("mex-workflowid", mexWorkflowId)
                        .WithHeader("mex-chunk-range", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
                        .WithHeader("mex-clientversion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("mex-osname", this.meshConfigurations.MexOSName)
                        .WithHeader("mex-osversion", this.meshConfigurations.MexOSVersion))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithBody(serialisedResponseMessage));

            using MemoryStream inputStream = new MemoryStream(contentBytes);

            // when
            Message actualSendMessageResult = await this.meshClient.Mailbox
                .SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    inputStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

            // then
            actualSendMessageResult.MessageId.Should().BeEquivalentTo(expectedSendMessageResult.MessageId);
        }
    }
}