// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputFileMessage);

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
