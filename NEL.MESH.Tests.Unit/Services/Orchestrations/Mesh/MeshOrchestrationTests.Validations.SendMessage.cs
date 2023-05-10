// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
                new NullMeshMessageException();

            var expectedMeshOrchestrationValidationException =
                new MeshOrchestrationValidationException(nullMeshMessageException);

            // when
            ValueTask<Message> messageTask = this.meshOrchestrationService
                .SendMessageAsync(message: nullMessage);

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
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;
            int randomChunkCount = GetRandomNumber();
            List<Message> randomChunkedMessages = CreateRandomChunkedSendMessages(randomChunkCount);
            List<Message> chunkedInputMessages = randomChunkedMessages;
            List<Message> chunkedOutputMessages = chunkedInputMessages.DeepClone();
            string randomMessageId = GetRandomString();
            chunkedOutputMessages[0].MessageId = randomMessageId;
            Message outputMessage = chunkedOutputMessages[0].DeepClone();
            outputMessage.StringContent = inputMessage.StringContent;
            Message expectedMessage = outputMessage.DeepClone();

            this.chunkServiceMock.Setup(service =>
                service.SplitMessageIntoChunks(inputMessage))
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
                .SendMessageAsync(message: randomMessage);

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messageTask.AsTask);

            // then
            actualMeshOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationValidationException);

            this.chunkServiceMock.Verify(service =>
                service.SplitMessageIntoChunks(inputMessage),
                    Times.Once);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

    }
}
