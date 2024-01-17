// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnTrackMessageIfMessageIdIsNullAndLogItAsync(string invalidText)
        {
            // given
            string invalidMessageId = invalidText;
            string randomToken = GetRandomString();
            Message randomMessage = CreateRandomSendMessage();

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
                .TrackMessageAsync(messageId: invalidMessageId);

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
        public async Task ShouldThrowValidationExceptionOnTrackMessageIfTokenIsNullAndLogItAsync(string invalidText)
        {
            // given
            string randomMssageId = GetRandomString();
            string invalidToken = invalidText;
            Message randomMessage = CreateRandomSendMessage();
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
                .TrackMessageAsync(messageId: randomMssageId);

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
