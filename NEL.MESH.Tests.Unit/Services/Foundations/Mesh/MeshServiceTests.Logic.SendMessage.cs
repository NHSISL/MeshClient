// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldSendFirstChunkPartAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            string chunkSize = "{1:2}";
            Message randomMessage = CreateRandomSendMessage(chunkSize);
            Message inputMessage = randomMessage;

            Dictionary<string, List<string>> contentHeaders = new Dictionary<string, List<string>>
            {
                { "Content-Type", new List<string>() { "text/plain" }},
                { "Content-Encoding", new List<string>() },
                { "Mex-FileName", new List<string>() },
                { "Mex-From", new List<string>() },
                { "Mex-To", new List<string>() },
                { "Mex-WorkflowID", new List<string>() },
                { "Mex-Chunk-Range", new List<string>() },
                { "Mex-LocalID", new List<string>() },
                { "Mex-Subject", new List<string>() },
                { "Mex-Content-Checksum", new List<string>() },
                { "Mex-Content-Encrypted", new List<string>() },
                { "Mex-ClientVersion", new List<string>() },
                { "Mex-OSVersion", new List<string>() },
                { "Mex-OSArchitecture", new List<string>() },
                { "Mex-JavaVersion", new List<string>() }
            };

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessageForSendMessage(inputMessage, contentHeaders);

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    GetKeyStringValue("Mex-To", inputMessage.Headers),
                    GetKeyStringValue("Mex-WorkflowID", inputMessage.Headers),
                    GetKeyStringValue("Mex-LocalID", inputMessage.Headers),
                    GetKeyStringValue("Mex-Subject", inputMessage.Headers),
                    GetKeyStringValue("Mex-FileName", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Checksum", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Encrypted", inputMessage.Headers),
                    GetKeyStringValue("Mex-Encoding", inputMessage.Headers),
                    GetKeyStringValue("Mex-Chunk-Range", inputMessage.Headers),
                    GetKeyStringValue("Content-Type", inputMessage.Headers),
                    authorizationToken,
                    inputMessage.StringContent))
                        .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageWithStringContentFromHttpResponseMessageForSend(responseMessage);

            // when
            var actualMessage = await this.meshService.SendMessageAsync(inputMessage, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    GetKeyStringValue("Mex-To", inputMessage.Headers),
                    GetKeyStringValue("Mex-WorkflowID", inputMessage.Headers),
                    GetKeyStringValue("Mex-LocalID", inputMessage.Headers),
                    GetKeyStringValue("Mex-Subject", inputMessage.Headers),
                    GetKeyStringValue("Mex-FileName", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Checksum", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Encrypted", inputMessage.Headers),
                    GetKeyStringValue("Mex-Encoding", inputMessage.Headers),
                    GetKeyStringValue("Mex-Chunk-Range", inputMessage.Headers),
                    GetKeyStringValue("Content-Type", inputMessage.Headers),
                    authorizationToken,
                    inputMessage.StringContent),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSendSecondChunkPartAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            string chunkSize = "{2:2}";
            Message randomMessage = CreateRandomSendMessage(chunkSize);
            Message inputMessage = randomMessage;
            string chunkPart = "2";

            Dictionary<string, List<string>> contentHeaders = new Dictionary<string, List<string>>
            {
                { "Content-Type", new List<string>() { "text/plain" }},
                { "Content-Encoding", new List<string>() },
                { "Mex-FileName", new List<string>() },
                { "Mex-From", new List<string>() },
                { "Mex-To", new List<string>() },
                { "Mex-WorkflowID", new List<string>() },
                { "Mex-Chunk-Range", new List<string>{} },
                { "Mex-LocalID", new List<string>() },
                { "Mex-Subject", new List<string>() },
                { "Mex-Content-Checksum", new List<string>() },
                { "Mex-Content-Encrypted", new List<string>() },
                { "Mex-ClientVersion", new List<string>() },
                { "Mex-OSVersion", new List<string>() },
                { "Mex-OSArchitecture", new List<string>() },
                { "Mex-JavaVersion", new List<string>() }
            };

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessage(inputMessage, contentHeaders);

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    GetKeyStringValue("Mex-To", inputMessage.Headers),
                    GetKeyStringValue("Mex-WorkflowID", inputMessage.Headers),
                    GetKeyStringValue("Mex-LocalID", inputMessage.Headers),
                    GetKeyStringValue("Mex-Subject", inputMessage.Headers),
                    GetKeyStringValue("Mex-FileName", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Checksum", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Encrypted", inputMessage.Headers),
                    GetKeyStringValue("Mex-Encoding", inputMessage.Headers),
                    GetKeyStringValue("Mex-Chunk-Range", inputMessage.Headers),
                    GetKeyStringValue("Content-Type", inputMessage.Headers),
                    authorizationToken,
                    inputMessage.StringContent,
                    inputMessage.MessageId,
                    chunkPart))
                        .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageWithStringContentFromHttpResponseMessage(responseMessage);

            // when
            var actualMessage = await this.meshService.SendMessageAsync(inputMessage, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    GetKeyStringValue("Mex-To", inputMessage.Headers),
                    GetKeyStringValue("Mex-WorkflowID", inputMessage.Headers),
                    GetKeyStringValue("Mex-LocalID", inputMessage.Headers),
                    GetKeyStringValue("Mex-Subject", inputMessage.Headers),
                    GetKeyStringValue("Mex-FileName", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Checksum", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Encrypted", inputMessage.Headers),
                    GetKeyStringValue("Mex-Encoding", inputMessage.Headers),
                    GetKeyStringValue("Mex-Chunk-Range", inputMessage.Headers),
                    GetKeyStringValue("Content-Type", inputMessage.Headers),
                    authorizationToken,
                    inputMessage.StringContent,
                    inputMessage.MessageId,
                    chunkPart),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
