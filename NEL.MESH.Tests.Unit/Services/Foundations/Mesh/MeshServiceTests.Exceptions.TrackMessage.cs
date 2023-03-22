// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationResponseMessages))]
        public async Task ShouldThrowDependencyValidationExceptionIfServerErrorOccursOnTrackMessageAsync(
            HttpResponseMessage dependencyValidationResponseMessage)
        {
            try
            {
                // given
                Message someMessage = CreateRandomMessage();
                HttpResponseMessage response = dependencyValidationResponseMessage;

                this.meshBrokerMock.Setup(broker =>
                    broker.TrackMessageAsync(It.IsAny<string>()))
                        .ReturnsAsync(dependencyValidationResponseMessage);

                var httpRequestException =
                    new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

                var failedMeshClientException =
                    new FailedMeshClientException(httpRequestException);

                var expectedMeshDependencyValidationException =
                    new MeshDependencyValidationException(failedMeshClientException.InnerException as Xeption);

                // when
                ValueTask<Message> sendMessageTask =
                    this.meshService.TrackMessageAsync(someMessage.MessageId);

                MeshDependencyValidationException actualMeshDependencyValidationException =
                    await Assert.ThrowsAsync<MeshDependencyValidationException>(sendMessageTask.AsTask);

                // then
                actualMeshDependencyValidationException.Should().BeEquivalentTo(expectedMeshDependencyValidationException);

                this.meshBrokerMock.Verify(broker =>
                    broker.TrackMessageAsync(It.IsAny<string>()),
                        Times.Once);

                this.meshBrokerMock.VerifyNoOtherCalls();
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }
    }
}
