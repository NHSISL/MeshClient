// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(MeshDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAcknowledgeMessageIfDependencyValidationErrorOccursAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someMessageId = GetRandomString();

            var expectedMeshOrchestrationDependencyValidationException = 
                new MeshOrchestrationDependencyValidationException(
                    message: "Mesh orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> acknowledgeMessageTask = this.meshOrchestrationService.AcknowledgeMessageAsync(someMessageId);

            MeshOrchestrationDependencyValidationException actualMeshOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyValidationException>(acknowledgeMessageTask.AsTask);

            // then
            actualMeshOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationDependencyValidationException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MeshDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAcknowledgeMessageIfDependencyErrorOccursAsync(
           Xeption dependencyException)
        {
            // given
            string someMessageId = GetRandomString();

            var expectedMeshOrchestrationDependencyException = new MeshOrchestrationDependencyException(
                message: "Mesh orchestration dependency error occurred, fix the errors and try again.",
                innerException: dependencyException.InnerException as Xeption);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> acknowledgeMessageTask = this.meshOrchestrationService.AcknowledgeMessageAsync(someMessageId);

            MeshOrchestrationDependencyException actualMeshOrchestrationDependencyException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyException>(acknowledgeMessageTask.AsTask);

            // then
            actualMeshOrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationDependencyException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAcknowledgeMessageIfServiceErrorOccursAsync()
        {
            // given
            string someMessageId = GetRandomString();
            string someErrorMessage = GetRandomString();
            var serviceException = new Exception(someErrorMessage);

            var failedMeshOrchestrationServiceException = new FailedMeshOrchestrationServiceException(
                message: "Failed mesh orchestration service occurred, please contact support",
                innerException: serviceException);

            var expectedMeshOrchestrationServiceException = new MeshOrchestrationServiceException(
                message: "Mesh orchestration service error occurred, contact support.",
                innerException: failedMeshOrchestrationServiceException);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> acknowledgeMessageTask = this.meshOrchestrationService.AcknowledgeMessageAsync(someMessageId);

            MeshOrchestrationServiceException actualMeshOrchestrationServiceException =
                await Assert.ThrowsAsync<MeshOrchestrationServiceException>(acknowledgeMessageTask.AsTask);

            // then
            actualMeshOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationServiceException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
