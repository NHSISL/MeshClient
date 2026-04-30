// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveMessageStreamIfMessageIdIsInvalidAsync(
            string invalidText)
        {
            // given
            string invalidMessageId = invalidText;
            using MemoryStream outputStream = new MemoryStream();

            var invalidMeshOrchestrationArgsException = new InvalidMeshOrchestrationArgsException(
                message: "Invalid mesh orchestration argument validation errors occurred, " +
                "please correct the errors and try again.");

            invalidMeshOrchestrationArgsException.AddData(
                key: nameof(Message.MessageId),
                values: "Text is required");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidMeshOrchestrationArgsException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: invalidMessageId, content: outputStream);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Never);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveMessageStreamIfOutputStreamIsNullAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            Stream nullOutputStream = null;

            var invalidMeshOrchestrationArgsException = new InvalidMeshOrchestrationArgsException(
                message: "Invalid mesh orchestration argument validation errors occurred, " +
                "please correct the errors and try again.");

            invalidMeshOrchestrationArgsException.AddData(
                key: "output",
                values: "Stream is required, must be writable, seekable and must be empty");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidMeshOrchestrationArgsException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: randomMessageId, content: nullOutputStream);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Never);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveMessageStreamIfOutputStreamIsNotEmptyAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            using MemoryStream nonEmptyOutputStream = new MemoryStream(new byte[] { 1, 2, 3 });

            var invalidMeshOrchestrationArgsException = new InvalidMeshOrchestrationArgsException(
                message: "Invalid mesh orchestration argument validation errors occurred, " +
                "please correct the errors and try again.");

            invalidMeshOrchestrationArgsException.AddData(
                key: "output",
                values: "Stream is required, must be writable, seekable and must be empty");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidMeshOrchestrationArgsException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: randomMessageId, content: nonEmptyOutputStream);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Never);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveMessageStreamIfOutputStreamIsNotWritableAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            byte[] buffer = new byte[16];
            using MemoryStream backingStream = new MemoryStream(buffer, writable: false);

            var invalidMeshOrchestrationArgsException = new InvalidMeshOrchestrationArgsException(
                message: "Invalid mesh orchestration argument validation errors occurred, " +
                "please correct the errors and try again.");

            invalidMeshOrchestrationArgsException.AddData(
                key: "output",
                values: "Stream is required, must be writable, seekable and must be empty");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidMeshOrchestrationArgsException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: randomMessageId, content: backingStream);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Never);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveMessageStreamIfTokenIsInvalidAsync(
            string invalidText)
        {
            // given
            string randomMessageId = GetRandomString();
            string invalidToken = invalidText;
            using MemoryStream outputStream = new MemoryStream();
            var invalidTokenException = new InvalidTokenException(message: "Token is invalid.");

            invalidTokenException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidTokenException);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(invalidToken);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: randomMessageId, content: outputStream);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveMessageStreamIfChunkTokenIsInvalidAsync(
            string invalidText)
        {
            // given
            string randomMessageId = GetRandomString();
            string validToken = GetRandomString();
            string invalidToken = invalidText;
            using MemoryStream outputStream = new MemoryStream();

            Message chunk1Message = new Message();
            chunk1Message.Headers.Add("mex-chunk-range", new System.Collections.Generic.List<string> { "{1:2}" });

            var invalidTokenException = new InvalidTokenException(message: "Token is invalid.");

            invalidTokenException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidTokenException);

            this.tokenServiceMock.SetupSequence(service => service.GenerateTokenAsync())
                .ReturnsAsync(validToken)
                .ReturnsAsync(invalidToken);

            this.meshServiceMock
                .Setup(service =>
                    service.RetrieveMessageAsync(
                        randomMessageId,
                        validToken,
                        It.IsAny<System.IO.Stream>(),
                        1,
                        It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(chunk1Message);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: randomMessageId, content: outputStream);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Exactly(2));

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(
                    randomMessageId,
                    validToken,
                    It.IsAny<System.IO.Stream>(),
                    1,
                    It.IsAny<System.Threading.CancellationToken>()),
                        Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
