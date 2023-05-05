// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveMessagesIfDependencyValidationErrorOccurs(
            Xeption dependencyValidationException)
        {
            // given
            string authorizationToken = GetRandomString();

            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(authorizationToken))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<string>> RetrieveMessagesTask =
                this.meshProcessingService.RetrieveMessagesAsync(authorizationToken);

            MeshProcessingDependencyValidationException actualMeshProcessingDependencyValidationException =
                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(RetrieveMessagesTask.AsTask);

            // then
            actualMeshProcessingDependencyValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
