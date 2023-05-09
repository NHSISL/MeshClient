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
        public async Task ShouldThrowDependencyValidationExceptionOnAcknowledgeMessageIfDependencyValidationErrorOccurs(
            Xeption dependencyValidationException)
        {
            // given
            string someMessageId = GetRandomString();
            string someAuthorizationToken = GetRandomString();

            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> sendMessageTask =
                this.meshProcessingService.AcknowledgeMessageAsync(someMessageId, someAuthorizationToken);

            MeshProcessingDependencyValidationException actualMeshProcessingDependencyValidationException =
                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(sendMessageTask.AsTask);

            // then
            actualMeshProcessingDependencyValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAcknowledgeMessageIfDependencyErrorOccurs(
            Xeption dependencyException)
        {
            // given
            string someMessageId = GetRandomString();
            string someAuthorizationToken = GetRandomString();

            var expectedMeshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> sendMessageTask =
                this.meshProcessingService.AcknowledgeMessageAsync(someMessageId, someAuthorizationToken);

            MeshProcessingDependencyException actualMeshProcessingDependencyException =
                await Assert.ThrowsAsync<MeshProcessingDependencyException>(sendMessageTask.AsTask);

            // then
            actualMeshProcessingDependencyException.Should()
                .BeEquivalentTo(expectedMeshProcessingDependencyException);

            this.meshServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAcknowledgeMessageIfServiceErrorOccurs()
        {
            // given
            string someMessageId = GetRandomString();
            string someAuthorizationToken = GetRandomString();
            var serviceException = new Exception();

            var failedMeshProcessingServiceException =
                new FailedMeshProcessingServiceException(serviceException);

            var expectedMeshProcessingServiceException =
                new MeshProcessingServiceException(failedMeshProcessingServiceException);

            this.meshServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> sendMessageTask =
                this.meshProcessingService.AcknowledgeMessageAsync(someMessageId, someAuthorizationToken);

            MeshProcessingServiceException actualMeshProcessingServiceException =
                await Assert.ThrowsAsync<MeshProcessingServiceException>(sendMessageTask.AsTask);

            // then
            actualMeshProcessingServiceException.Should()
                .BeEquivalentTo(expectedMeshProcessingServiceException);

            this.meshServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}