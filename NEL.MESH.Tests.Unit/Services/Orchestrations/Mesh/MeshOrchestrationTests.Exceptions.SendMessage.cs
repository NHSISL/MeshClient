// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(MeshDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnSendMessageIfDependencyValidationErrorOccursAsync(
            Xeption dependencyValidationException)
        {
            // given
            Message someMessage = CreateRandomSendMessage();

            var expectedMeshOrchestrationDependencyValidationException =
                new MeshOrchestrationDependencyValidationException(
                    message: "Mesh orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.chunkServiceMock.Setup(service =>
                service.SplitMessageIntoChunks(It.IsAny<Message>()))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<Message> sendMessageTask = this.meshOrchestrationService.SendMessageAsync(someMessage);

            MeshOrchestrationDependencyValidationException actualMeshOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyValidationException>(sendMessageTask.AsTask);

            // then
            actualMeshOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationDependencyValidationException);

            this.chunkServiceMock.Verify(service =>
                service.SplitMessageIntoChunks(It.IsAny<Message>()),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MeshDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnSendMessageIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            Message someMessage = CreateRandomSendMessage();

            var expectedMeshOrchestrationDependencyException = new MeshOrchestrationDependencyException(
                message: "Mesh orchestration dependency error occurred, fix the errors and try again.",
                innerException: dependencyException.InnerException as Xeption);

            this.chunkServiceMock.Setup(service =>
                service.SplitMessageIntoChunks(It.IsAny<Message>()))
                    .Throws(dependencyException);

            // when
            ValueTask<Message> sendMessageTask = this.meshOrchestrationService.SendMessageAsync(someMessage);

            MeshOrchestrationDependencyException actualMeshOrchestrationDependencyException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyException>(sendMessageTask.AsTask);

            // then
            actualMeshOrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationDependencyException);

            this.chunkServiceMock.Verify(service =>
                service.SplitMessageIntoChunks(It.IsAny<Message>()),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnSendMessageIfServiceErrorOccursAsync()
        {
            // given
            Message someMessage = CreateRandomSendMessage();
            string someErrorMessage = GetRandomString();
            var serviceException = new Exception(someErrorMessage);

            var failedMeshOrchestrationServiceException = new FailedMeshOrchestrationServiceException(
                        message: "Failed mesh orchestration service occurred, please contact support",
                        innerException: serviceException);

            var expectedMeshOrchestrationServiceException = new MeshOrchestrationServiceException(
                    message: "Mesh orchestration service error occurred, contact support.",
                    innerException: failedMeshOrchestrationServiceException);

            this.chunkServiceMock.Setup(service =>
                service.SplitMessageIntoChunks(It.IsAny<Message>()))
                    .Throws(serviceException);

            // when
            ValueTask<Message> sendMessageTask = this.meshOrchestrationService.SendMessageAsync(someMessage);

            MeshOrchestrationServiceException actualMeshOrchestrationServiceException =
                await Assert.ThrowsAsync<MeshOrchestrationServiceException>(sendMessageTask.AsTask);

            // then
            actualMeshOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationServiceException);

            this.chunkServiceMock.Verify(service =>
                service.SplitMessageIntoChunks(It.IsAny<Message>()),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
