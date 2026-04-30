// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
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
            byte[] expectedBytes = new byte[] { 1, 2, 3 };

            Message serviceMessage = CreateRandomSendMessage();
            Message expectedMessage = serviceMessage.DeepClone();
            using MemoryStream outputStream = new MemoryStream();

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            this.meshServiceMock
                .Setup(service =>
                    service.RetrieveMessageAsync(
                        inputMessageId,
                        randomToken,
                        It.IsAny<Stream>(),
                        1))
                .Callback<string, string, Stream, int, CancellationToken>((_, _, stream, _, _) =>
                    stream.Write(expectedBytes, 0, expectedBytes.Length))
                .ReturnsAsync(serviceMessage);

            // when
            Message actualMessage = await this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: inputMessageId, content: outputStream);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);
            outputStream.ToArray().Should().BeEquivalentTo(expectedBytes);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(
                    inputMessageId,
                    randomToken,
                    It.IsAny<Stream>(),
                    1),
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
            chunk1Message.Headers["mex-chunk-range"] = new List<string> { "{1:2}" };

            Message chunk2Message = CreateRandomSendMessage();

            using MemoryStream outputStream = new MemoryStream();

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            this.meshServiceMock
                .Setup(service =>
                    service.RetrieveMessageAsync(
                        inputMessageId,
                        randomToken,
                        It.IsAny<Stream>(),
                        1))
                .Callback<string, string, Stream, int, CancellationToken>((_, _, stream, _, _) =>
                    stream.Write(chunk1Bytes, 0, chunk1Bytes.Length))
                .ReturnsAsync(chunk1Message);

            this.meshServiceMock
                .Setup(service =>
                    service.RetrieveMessageAsync(
                        inputMessageId,
                        randomToken,
                        It.IsAny<Stream>(),
                        2))
                .Callback<string, string, Stream, int, CancellationToken>((_, _, stream, _, _) =>
                    stream.Write(chunk2Bytes, 0, chunk2Bytes.Length))
                .ReturnsAsync(chunk2Message);

            // when
            Message actualMessage = await this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: inputMessageId, content: outputStream);

            // then
            actualMessage.Should().BeEquivalentTo(chunk1Message);
            outputStream.ToArray().Should().BeEquivalentTo(expectedBytes);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Exactly(2));

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(
                    inputMessageId,
                    randomToken,
                    It.IsAny<Stream>(),
                    1),
                        Times.Once);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(
                    inputMessageId,
                    randomToken,
                    It.IsAny<Stream>(),
                    2),
                        Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
