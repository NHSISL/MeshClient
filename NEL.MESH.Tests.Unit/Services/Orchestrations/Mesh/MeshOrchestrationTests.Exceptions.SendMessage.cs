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
            Xeption dependancyValidationException)
        {
            // given
            Message someMessage = CreateRandomSendMessage();
            var expectedMeshOrchestrationDependencyValidationException =
            new MeshOrchestrationDependencyValidationException(
                dependancyValidationException.InnerException as Xeption);

            this.tokenProcessingServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<Message> sendMessageTask = this.meshOrchestrationService.SendMessageAsync(someMessage);

            MeshOrchestrationDependencyValidationException actualMeshOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyValidationException>(sendMessageTask.AsTask);

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
        public async Task ShouldThrowDependencyExceptionOnSendMessageIfDependencyErrorOccursAsync(
            Xeption dependancyException)
        {
            // given
            Message someMessage = CreateRandomSendMessage();
            var expectedMeshOrchestrationDependencyException =
            new MeshOrchestrationDependencyException(
                dependancyException.InnerException as Xeption);

            this.tokenProcessingServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<Message> sendMessageTask = this.meshOrchestrationService.SendMessageAsync(someMessage);

            MeshOrchestrationDependencyException actualMeshOrchestrationDependencyException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyException>(sendMessageTask.AsTask);

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
        public async Task ShouldThrowServiceExceptionOnSendMessageIfServiceErrorOccursAsync()
        {
            // given
            Message someMessage = CreateRandomSendMessage();
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
            ValueTask<Message> sendMessageTask = this.meshOrchestrationService.SendMessageAsync(someMessage);

            MeshOrchestrationServiceException actualMeshOrchestrationServiceException =
                await Assert.ThrowsAsync<MeshOrchestrationServiceException>(sendMessageTask.AsTask);

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
