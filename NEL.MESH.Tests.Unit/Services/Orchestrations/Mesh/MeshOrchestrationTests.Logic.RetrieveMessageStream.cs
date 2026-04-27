// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetrieveMessageToStreamAsync()
        {
            // given
            string randomToken = GetRandomString();
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            Message randomMessage = CreateRandomSendMessage();
            randomMessage.FileContent = new byte[] { 1, 2, 3 };
            Message serviceMessage = randomMessage.DeepClone();
            Message expectedMessage = randomMessage.DeepClone();
            expectedMessage.FileContent = null;
            using MemoryStream outputStream = new MemoryStream();

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageAsync(inputMessageId, randomToken, 1))
                    .ReturnsAsync(serviceMessage);

            // when
            Message actualMessage = await this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: inputMessageId, outputStream: outputStream);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);
            outputStream.ToArray().Should().BeEquivalentTo(randomMessage.FileContent);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(inputMessageId, randomToken, 1),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveChunkedMessageToStreamAsync()
        {
            // given
            string randomToken = GetRandomString();
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            byte[] chunk1Bytes = new byte[] { 1, 2, 3 };
            byte[] chunk2Bytes = new byte[] { 4, 5, 6 };
            byte[] expectedBytes = new byte[] { 1, 2, 3, 4, 5, 6 };

            Message chunk1Message = CreateRandomSendMessage();
            chunk1Message.FileContent = chunk1Bytes;
            chunk1Message.Headers["mex-chunk-range"] = new System.Collections.Generic.List<string> { "{1:2}" };

            Message chunk2Message = CreateRandomSendMessage();
            chunk2Message.FileContent = chunk2Bytes;

            using MemoryStream outputStream = new MemoryStream();

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageAsync(inputMessageId, randomToken, 1))
                    .ReturnsAsync(chunk1Message);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageAsync(inputMessageId, randomToken, 2))
                    .ReturnsAsync(chunk2Message);

            // when
            Message actualMessage = await this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: inputMessageId, outputStream: outputStream);

            // then
            actualMessage.FileContent.Should().BeNull();
            outputStream.ToArray().Should().BeEquivalentTo(expectedBytes);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Exactly(2));

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(inputMessageId, randomToken, 1),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(inputMessageId, randomToken, 2),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
