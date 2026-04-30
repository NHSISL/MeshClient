// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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
        public async Task ShouldThrowValidationExceptionOnHandshakeIfTokenIsInvalidAsync(string invalidToken)
        {
            // given
            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(invalidToken);

            var invalidTokenException = new InvalidTokenException(message: "Token is invalid.");

            invalidTokenException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshOrchestrationValidationException = new MeshOrchestrationValidationException(
                message: "Mesh orchestration validation errors occurred, please try again.",
                innerException: invalidTokenException);

            // when
            ValueTask<bool> handshakeTask = this.meshOrchestrationService.HandshakeAsync();

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(handshakeTask.AsTask);

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
