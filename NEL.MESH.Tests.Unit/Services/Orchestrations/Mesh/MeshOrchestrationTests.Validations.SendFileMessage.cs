// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendFileIfMessageIsNullAndLogItAsync()
        {
            // given
            Message nullMessage = null;

            var nullMeshMessageException =
                new NullMeshMessageException();

            var expectedMeshOrchestrationValidationException =
                new MeshOrchestrationValidationException(nullMeshMessageException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .SendFileAsync(message: nullMessage);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnSendFileIfTokenIsNullAndLogItAsync(string invalidText)
        {
            // given
            string invalidToken = invalidText;
            string randomToken = GetRandomString();
            Message randomMessage = CreateRandomSendFileMessage();
            Message inputMessage = randomMessage;
            int randomChunkCount = GetRandomNumber();
            List<Message> randomChunkedMessages = CreateRandomChunkedSendFileMessages(randomChunkCount);
            List<Message> chunkedInputMessages = randomChunkedMessages;

            this.chunkServiceMock.Setup(service =>
                service.SplitFileMessageIntoChunks(inputMessage))
                    .Returns(chunkedInputMessages);

            var invalidTokenException =
                new InvalidTokenException();

            invalidTokenException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshOrchestrationValidationException =
                new MeshOrchestrationValidationException(innerException: invalidTokenException);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(invalidToken);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .SendFileAsync(message: randomMessage);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.chunkServiceMock.Verify(service =>
                service.SplitFileMessageIntoChunks(inputMessage),
                    Times.Once);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMessageList))]
        public async Task ShouldThrowValidationExceptionOnSendFileMessageIfChunksIsNullOrEmptyAndLogItAsync(
            List<Message> invalidData)
        {
            // given
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;

            this.chunkServiceMock.Setup(service =>
                service.SplitFileMessageIntoChunks(inputMessage))
                    .Returns(invalidData);

            var invalidMeshOrchestrationArgsException =
                new InvalidMeshOrchestrationArgsException();

            invalidMeshOrchestrationArgsException.AddData(
                key: "ChunkedMessages",
                values: "At least one chunk part required");

            var expectedMeshOrchestrationValidationException =
                new MeshOrchestrationValidationException(innerException: invalidMeshOrchestrationArgsException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .SendFileAsync(message: randomMessage);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.chunkServiceMock.Verify(service =>
                service.SplitFileMessageIntoChunks(inputMessage),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

    }
}
