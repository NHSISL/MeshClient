// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Processings.Mesh;
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
    }
}