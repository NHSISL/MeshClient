// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveMessagesIfDependencyValidationErrorOccursAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedMeshOrchestrationDependencyValidationException =
            new MeshOrchestrationDependencyValidationException(
                dependancyValidationException.InnerException as Xeption);

            this.tokenProcessingServiceMock.Setup(service =>
              service.GenerateTokenAsync())
                  .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<List<string>> retrieveMessagesTask = this.meshOrchestrationService.RetrieveMessagesAsync();

            MeshOrchestrationDependencyValidationException actualMeshOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyValidationException>(retrieveMessagesTask.AsTask);

            // then
            actualMeshOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationDependencyValidationException);

            this.tokenProcessingServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.tokenProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MeshDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveMessagesIfDependencyErrorOccursAsync(
           Xeption dependancyException)
        {
            // given
            var expectedMeshOrchestrationDependencyException =
            new MeshOrchestrationDependencyException(
                dependancyException.InnerException as Xeption);

            this.tokenProcessingServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<List<string>> retrieveMessagesTask = this.meshOrchestrationService.RetrieveMessagesAsync();

            MeshOrchestrationDependencyException actualMeshOrchestrationDependencyException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyException>(retrieveMessagesTask.AsTask);

            // then
            actualMeshOrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationDependencyException);

            this.tokenProcessingServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.tokenProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveMessagesIfServiceErrorOccursAsync()
        {
            // given
            string someErrorMessage = GetRandomString();
            var serviceException = new Exception(someErrorMessage);

            var failedMeshOrchestrationServiceException =
                new FailedMeshOrchestrationServiceException(serviceException);

            var expectedMeshOrchestrationServiceException =
            new MeshOrchestrationServiceException(failedMeshOrchestrationServiceException);

            this.tokenProcessingServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<string>> recieveMessagesTask = this.meshOrchestrationService.RetrieveMessagesAsync();

            MeshOrchestrationServiceException actualMeshOrchestrationServiceException =
                await Assert.ThrowsAsync<MeshOrchestrationServiceException>(recieveMessagesTask.AsTask);

            // then
            actualMeshOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationServiceException);

            this.tokenProcessingServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.tokenProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
