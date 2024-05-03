// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSISL.MESH.Models.Foundations.Mesh;
using NHSISL.MESH.Models.Orchestrations.Mesh.Exceptions;
using Xunit;

namespace NHSISL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveMessagesIfTokenIsNullAndLogItAsync(string invalidText)
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
            ValueTask<List<string>> messagesTask = this.meshOrchestrationService
                .RetrieveMessagesAsync();

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messagesTask.AsTask);

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
