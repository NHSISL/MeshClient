// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSISL.MESH.Models.Orchestrations.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace NHSISL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(MeshDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnHandshakeIfDependencyValidationErrorOccursAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedMeshOrchestrationDependencyValidationException =
                new MeshOrchestrationDependencyValidationException(
                    message: "Mesh orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> handshakeTask = this.meshOrchestrationService.HandshakeAsync();

            MeshOrchestrationDependencyValidationException actualMeshOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyValidationException>(handshakeTask.AsTask);

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
        public async Task ShouldThrowDependencyExceptionOnHandshakeIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            var expectedMeshOrchestrationDependencyException = new MeshOrchestrationDependencyException(
                message: "Mesh orchestration dependency error occurred, fix the errors and try again.",
                innerException: dependencyException.InnerException as Xeption);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> handshakeTask = this.meshOrchestrationService.HandshakeAsync();

            MeshOrchestrationDependencyException actualMeshOrchestrationDependencyException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyException>(handshakeTask.AsTask);

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
        public async Task ShouldThrowServiceExceptionOnHandshakeIfServiceErrorOccursAsync()
        {
            // given
            string someMessage = GetRandomString();
            var serviceException = new Exception(someMessage);

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
            ValueTask<bool> handshakeTask = this.meshOrchestrationService.HandshakeAsync();

            MeshOrchestrationServiceException actualMeshOrchestrationServiceException =
                await Assert.ThrowsAsync<MeshOrchestrationServiceException>(handshakeTask.AsTask);

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
