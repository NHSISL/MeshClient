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
        public async Task ShouldThrowDependencyValidationExceptionOnHandshakeIfDependencyValidationErrorOccursAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedMeshOrchestrationDependencyValidationException =
            new MeshOrchestrationDependencyValidationException(
                dependancyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.HandshakeAsync())
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<bool> handshakeTask = this.meshOrchestrationService.HandshakeAsync();

            MeshOrchestrationDependencyValidationException actualMeshOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyValidationException>(handshakeTask.AsTask);

            // then
            actualMeshOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationDependencyValidationException);

            this.meshServiceMock.Verify(service =>
                service.HandshakeAsync(),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MeshDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnHandshakeIfDependencyErrorOccursAsync(
            Xeption dependancyException)
        {
            // given
            var expectedMeshOrchestrationDependencyException =
            new MeshOrchestrationDependencyException(
                dependancyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.HandshakeAsync())
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<bool> handshakeTask = this.meshOrchestrationService.HandshakeAsync();

            MeshOrchestrationDependencyException actualMeshOrchestrationDependencyException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyException>(handshakeTask.AsTask);

            // then
            actualMeshOrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationDependencyException);

            this.meshServiceMock.Verify(service =>
                service.HandshakeAsync(),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnHandshakeIfServiceErrorOccursAsync()
        {
            // given
            string someMessage = GetRandomString();
            var serviceException = new Exception(someMessage);

            var failedMeshOrchestrationServiceException =
                new FailedMeshOrchestrationServiceException(serviceException);


            var expectedMeshOrchestrationServiceException =
            new MeshOrchestrationServiceException(failedMeshOrchestrationServiceException);

            this.meshServiceMock.Setup(service =>
                service.HandshakeAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> handshakeTask = this.meshOrchestrationService.HandshakeAsync();

            MeshOrchestrationServiceException actualMeshOrchestrationServiceException =
                await Assert.ThrowsAsync<MeshOrchestrationServiceException>(handshakeTask.AsTask);

            // then
            actualMeshOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationServiceException);

            this.meshServiceMock.Verify(service =>
                service.HandshakeAsync(),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
