// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Processings.Mesh;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnSendMessageIfDependencyValidationErrorOccurs(
            Xeption dependencyValidationException)
        {
            // given
            string someAuthorizationToken = GetRandomString();
            Message someMessage = CreateRandomMessage();

            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(It.IsAny<Message>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<Message> sendMessageTask =
                this.meshProcessingService.SendMessageAsync(someMessage, someAuthorizationToken);

            MeshProcessingDependencyValidationException actualMeshProcessingDependencyValidationException =
                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(sendMessageTask.AsTask);

            // then
            actualMeshProcessingDependencyValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(It.IsAny<Message>(), It.IsAny<string>()),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnSendMessageIfDependencyErrorOccurs(
            Xeption dependencyException)
        {
            // given
            string someAuthorizationToken = GetRandomString();
            Message someMessage = CreateRandomMessage();

            var expectedMeshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(It.IsAny<Message>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<Message> sendMessageTask =
                this.meshProcessingService.SendMessageAsync(someMessage, someAuthorizationToken);

            MeshProcessingDependencyException actualMeshProcessingDependencyException =
                await Assert.ThrowsAsync<MeshProcessingDependencyException>(sendMessageTask.AsTask);

            // then
            actualMeshProcessingDependencyException.Should()
                .BeEquivalentTo(expectedMeshProcessingDependencyException);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(It.IsAny<Message>(), It.IsAny<string>()),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}