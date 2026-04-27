// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
                .RetrieveMessageAsync(messageId: invalidMessageId, outputStream: outputStream);

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
                values: "Stream is required and must be empty");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidMeshOrchestrationArgsException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: randomMessageId, outputStream: nullOutputStream);

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
                values: "Stream is required and must be empty");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidMeshOrchestrationArgsException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: randomMessageId, outputStream: nonEmptyOutputStream);

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
                .RetrieveMessageAsync(messageId: randomMessageId, outputStream: outputStream);

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
    }
}
