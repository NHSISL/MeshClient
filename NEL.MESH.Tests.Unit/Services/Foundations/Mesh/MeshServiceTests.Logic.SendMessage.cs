// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
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
        public async Task ShouldSendMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    GetKeyStringValue("Mex-To", inputMessage.Headers),
                    GetKeyStringValue("Mex-WorkflowID", inputMessage.Headers),
                    inputMessage.StringContent,
                    GetKeyStringValue("Content-Type", inputMessage.Headers),
                    GetKeyStringValue("Mex-LocalID", inputMessage.Headers),
                    GetKeyStringValue("Mex-Subject", inputMessage.Headers),
                    GetKeyStringValue("Mex-FileName", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Checksum", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Encrypted", inputMessage.Headers),
                    GetKeyStringValue("Mex-Encoding", inputMessage.Headers),
                    GetKeyStringValue("Mex-Chunk-Range", inputMessage.Headers),
                    authorizationToken
                    ))
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
                    inputMessage.StringContent,
                    GetKeyStringValue("Content-Type", inputMessage.Headers),
                    GetKeyStringValue("Mex-LocalID", inputMessage.Headers),
                    GetKeyStringValue("Mex-Subject", inputMessage.Headers),
                    GetKeyStringValue("Mex-FileName", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Checksum", inputMessage.Headers),
                    GetKeyStringValue("Mex-Content-Encrypted", inputMessage.Headers),
                    GetKeyStringValue("Mex-Encoding", inputMessage.Headers),
                    GetKeyStringValue("Mex-Chunk-Range", inputMessage.Headers),
                    authorizationToken),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
