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

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.tokenProcessingServiceMock.VerifyNoOtherCalls();
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

            var invalidTokenException =
                new InvalidTokenException();

            invalidTokenException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshOrchestrationValidationException =
                new MeshOrchestrationValidationException(innerException: invalidTokenException);

            this.tokenProcessingServiceMock.Setup(service =>
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

            this.tokenProcessingServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.tokenProcessingServiceMock.VerifyNoOtherCalls();
        }

    }
}
