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
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfMessageIsNullAndLogItAsync()
        {
            // given
            Message nullMessage = null;

            var nullMeshMessageException =
                new NullMeshMessageException(message: "Message is null.");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: nullMeshMessageException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .SendMessageAsync(message: nullMessage, content: new MemoryStream());

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
        public async Task ShouldThrowValidationExceptionOnSendMessageIfTokenIsNullAndLogItAsync(string invalidText)
        {
            // given
            string invalidToken = invalidText;
            byte[] randomBytes = new byte[] { 1, 2, 3 };
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;
            using MemoryStream inputStream = new MemoryStream(randomBytes);

            this.chunkServiceMock.Setup(service =>
                service.SplitStreamIntoChunks(inputMessage, inputStream))
                    .Returns(new List<(Message, byte[])>
                    {
                        (CreateRandomSendMessage(), randomBytes)
                    });

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
                .SendMessageAsync(message: randomMessage, content: inputStream);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.chunkServiceMock.Verify(service =>
                service.SplitStreamIntoChunks(inputMessage, inputStream),
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
        public async Task ShouldThrowValidationExceptionOnSendMessageMessageIfChunksIsNullOrEmptyAndLogItAsync(
            List<Message> invalidData)
        {
            // given
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;
            byte[] randomBytes = new byte[] { 1, 2, 3 };
            using MemoryStream inputStream = new MemoryStream(randomBytes);

            this.chunkServiceMock.Setup(service =>
                service.SplitStreamIntoChunks(inputMessage, inputStream))
                    .Returns(new List<(Message, byte[])>());

            var invalidMeshOrchestrationArgsException = new InvalidMeshOrchestrationArgsException(
                message: "Invalid mesh orchestration argument validation errors occurred, " +
                "please correct the errors and try again.");

            invalidMeshOrchestrationArgsException.AddData(
                key: "ChunkedMessages",
                values: "At least one chunk part required");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidMeshOrchestrationArgsException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .SendMessageAsync(message: randomMessage, content: inputStream);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.chunkServiceMock.Verify(service =>
                service.SplitStreamIntoChunks(inputMessage, inputStream),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
