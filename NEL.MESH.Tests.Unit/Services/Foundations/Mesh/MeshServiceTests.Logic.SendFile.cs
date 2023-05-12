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
        public async Task ShouldSendFileAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message randomFileMessage = CreateRandomSendFileMessage();
            Message inputFileMessage = randomFileMessage;

            Dictionary<string, List<string>> contentHeaders = new Dictionary<string, List<string>>
            {
                { "Content-Type", new List<string>() { "application/octet-stream" }},
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

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessageForSendMessage(inputFileMessage, contentHeaders);

            this.meshBrokerMock.Setup(broker =>
                broker.SendFileAsync(
                    GetKeyStringValue("Mex-To", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-WorkflowID", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-LocalID", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Subject", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-FileName", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Content-Checksum", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Content-Encrypted", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Encoding", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Chunk-Range", inputFileMessage.Headers),
                    GetKeyStringValue("Content-Type", inputFileMessage.Headers),
                    authorizationToken,
                    inputFileMessage.FileContent))
                        .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageWithFileContentFromHttpResponseMessage(responseMessage);

            // when
            Message actualMessage = await this.meshService.SendFileAsync(inputFileMessage, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(
                    GetKeyStringValue("Mex-To", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-WorkflowID", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-LocalID", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Subject", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-FileName", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Content-Checksum", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Content-Encrypted", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Encoding", inputFileMessage.Headers),
                    GetKeyStringValue("Mex-Chunk-Range", inputFileMessage.Headers),
                    GetKeyStringValue("Content-Type", inputFileMessage.Headers),
                    authorizationToken,
                    inputFileMessage.FileContent),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
