// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSISL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NHSISL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldSendFirstChunkPartFileMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            string chunkSize = "{1:2}";
            Message randomFileMessage = CreateRandomSendMessage(chunkSize);
            Message inputFileMessage = randomFileMessage;

            Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>
            {
                { "mex-from", new List<string>() },
                { "mex-to", new List<string>() },
                { "mex-workflowid", new List<string>() },
                { "mex-chunk-range", new List<string>() },
                { "mex-subject", new List<string>() },
                { "mex-localid", new List<string>() },
                { "mex-filename", new List<string>() },
                { "mex-content-checksum", new List<string>() },
                { "content-type", new List<string>() { "application/octet-stream" }},
                { "content-encoding", new List<string>() },
                { "Accept", new List<string>() },
                { "mex-clientversion", new List<string>() },
                { "mex-osversion", new List<string>() },
                { "mex-osarchitecture", new List<string>() },
                { "mex-javaversion", new List<string>() }
            };

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessageForSendMessage(
                inputFileMessage, headers);

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    authorizationToken,
                    GetKeyStringValue("mex-from", inputFileMessage.Headers),
                    GetKeyStringValue("mex-to", inputFileMessage.Headers),
                    GetKeyStringValue("mex-workflowid", inputFileMessage.Headers),
                    GetKeyStringValue("mex-chunk-range", inputFileMessage.Headers),
                    GetKeyStringValue("mex-subject", inputFileMessage.Headers),
                    GetKeyStringValue("mex-localid", inputFileMessage.Headers),
                    GetKeyStringValue("mex-filename", inputFileMessage.Headers),
                    GetKeyStringValue("mex-content-checksum", inputFileMessage.Headers),
                    GetKeyStringValue("content-type", inputFileMessage.Headers),
                    GetKeyStringValue("content-encoding", inputFileMessage.Headers),
                    GetKeyStringValue("accept", inputFileMessage.Headers),
                    inputFileMessage.FileContent))
                        .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageFromHttpResponseMessage(
                responseMessage, inputFileMessage);

            // when
            Message actualMessage = await this.meshService.SendMessageAsync(inputFileMessage, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    authorizationToken,
                    GetKeyStringValue("mex-from", inputFileMessage.Headers),
                    GetKeyStringValue("mex-to", inputFileMessage.Headers),
                    GetKeyStringValue("mex-workflowid", inputFileMessage.Headers),
                    GetKeyStringValue("mex-chunk-range", inputFileMessage.Headers),
                    GetKeyStringValue("mex-subject", inputFileMessage.Headers),
                    GetKeyStringValue("mex-localid", inputFileMessage.Headers),
                    GetKeyStringValue("mex-filename", inputFileMessage.Headers),
                    GetKeyStringValue("mex-content-checksum", inputFileMessage.Headers),
                    GetKeyStringValue("content-type", inputFileMessage.Headers),
                    GetKeyStringValue("content-encoding", inputFileMessage.Headers),
                    GetKeyStringValue("accept", inputFileMessage.Headers),
                    inputFileMessage.FileContent),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSendSecondChunkPartFileAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            string chunkSize = "{2:2}";
            Message randomFileMessage = CreateRandomSendMessage(chunkSize);
            Message inputFileMessage = randomFileMessage;
            string chunkPart = "2";

            Dictionary<string, List<string>> contentHeaders = new Dictionary<string, List<string>>
            {
                { "mex-from", new List<string>() },
                { "mex-to", new List<string>() },
                { "mex-workflowid", new List<string>() },
                { "mex-chunk-range", new List<string>() },
                { "mex-subject", new List<string>() },
                { "mex-localid", new List<string>() },
                { "mex-filename", new List<string>() },
                { "mex-content-checksum", new List<string>() },
                { "content-type", new List<string>() { "application/octet-stream" }},
                { "content-encoding", new List<string>() },
                { "accept", new List<string>() },
                { "mex-clientversion", new List<string>() },
                { "mex-osversion", new List<string>() },
                { "mex-osarchitecture", new List<string>() },
                { "mex-javaversion", new List<string>() }
            };

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessageForSendMessage(
                inputFileMessage, contentHeaders);

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    authorizationToken,
                    GetKeyStringValue("mex-from", inputFileMessage.Headers),
                    GetKeyStringValue("mex-to", inputFileMessage.Headers),
                    GetKeyStringValue("mex-workflowid", inputFileMessage.Headers),
                    GetKeyStringValue("mex-chunk-range", inputFileMessage.Headers),
                    GetKeyStringValue("mex-subject", inputFileMessage.Headers),
                    GetKeyStringValue("mex-localid", inputFileMessage.Headers),
                    GetKeyStringValue("mex-filename", inputFileMessage.Headers),
                    GetKeyStringValue("mex-content-checksum", inputFileMessage.Headers),
                    GetKeyStringValue("content-type", inputFileMessage.Headers),
                    GetKeyStringValue("content-encoding", inputFileMessage.Headers),
                    GetKeyStringValue("accept", inputFileMessage.Headers),
                    inputFileMessage.FileContent,
                    inputFileMessage.MessageId,
                    chunkPart))
                        .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageFromHttpResponseMessage(
                responseMessage, inputFileMessage);

            // when
            Message actualMessage = await this.meshService.SendMessageAsync(inputFileMessage, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    authorizationToken,
                    GetKeyStringValue("mex-from", inputFileMessage.Headers),
                    GetKeyStringValue("mex-to", inputFileMessage.Headers),
                    GetKeyStringValue("mex-workflowid", inputFileMessage.Headers),
                    GetKeyStringValue("mex-chunk-range", inputFileMessage.Headers),
                    GetKeyStringValue("mex-subject", inputFileMessage.Headers),
                    GetKeyStringValue("mex-localid", inputFileMessage.Headers),
                    GetKeyStringValue("mex-filename", inputFileMessage.Headers),
                    GetKeyStringValue("mex-content-checksum", inputFileMessage.Headers),
                    GetKeyStringValue("content-type", inputFileMessage.Headers),
                    GetKeyStringValue("content-encoding", inputFileMessage.Headers),
                    GetKeyStringValue("accept", inputFileMessage.Headers),
                    inputFileMessage.FileContent,
                    inputFileMessage.MessageId,
                    chunkPart),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
