// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Processings.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnHandshakeIfDependencyValidationErrorOccurs(
            Xeption dependencyValidationException)
        {
            // given
            string authorizationToken = GetRandomString();

            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.HandshakeAsync(authorizationToken))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> HandshakeTask =
                this.meshProcessingService.HandshakeAsync(authorizationToken);

            MeshProcessingDependencyValidationException actualMeshProcessingDependencyValidationException =
                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(HandshakeTask.AsTask);

            // then
            actualMeshProcessingDependencyValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
                service.HandshakeAsync(authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnHandshakeIfDependencyErrorOccurs(
            Xeption dependencyException)
        {
            // given
            string authorizationToken = GetRandomString();

            var expectedMeshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.HandshakeAsync(authorizationToken))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> HandshakeTask =
                this.meshProcessingService.HandshakeAsync(authorizationToken);

            MeshProcessingDependencyException actualMeshProcessingDependencyException =
                await Assert.ThrowsAsync<MeshProcessingDependencyException>(HandshakeTask.AsTask);

            // then
            actualMeshProcessingDependencyException.Should()
                .BeEquivalentTo(expectedMeshProcessingDependencyException);

            this.meshServiceMock.Verify(service =>
                service.HandshakeAsync(authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnHandshakeIfServiceErrorOccurs()
        {
            // given
            string authorizationToken = GetRandomString();
            var serviceException = new Exception();

            var failedMeshProcessingServiceException =
                new FailedMeshProcessingServiceException(serviceException);

            var expectedMeshProcessingServiceException =
                new MeshProcessingServiceException(failedMeshProcessingServiceException);

            this.meshServiceMock.Setup(service =>
                service.HandshakeAsync(authorizationToken))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> HandshakeTask =
                this.meshProcessingService.HandshakeAsync(authorizationToken);

            MeshProcessingServiceException actualMeshProcessingServiceException =
                await Assert.ThrowsAsync<MeshProcessingServiceException>(HandshakeTask.AsTask);

            // then
            actualMeshProcessingServiceException.Should()
                .BeEquivalentTo(expectedMeshProcessingServiceException);

            this.meshServiceMock.Verify(service =>
                service.HandshakeAsync(authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
