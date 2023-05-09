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
            ValueTask<List<string>> messagesTask = this.meshOrchestrationService
                .RetrieveMessagesAsync();

            MeshOrchestrationValidationException actualMeshOrchestrationValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationValidationException>(messagesTask.AsTask);

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
