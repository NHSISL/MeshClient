// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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

            this.tokenServiceMock.Setup(service =>
              service.GenerateTokenAsync())
                  .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<List<string>> retrieveMessagesTask = this.meshOrchestrationService.RetrieveMessagesAsync();

            MeshOrchestrationDependencyValidationException actualMeshOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<MeshOrchestrationDependencyValidationException>(retrieveMessagesTask.AsTask);

            // then
            actualMeshOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedMeshOrchestrationDependencyValidationException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }

    }
}
