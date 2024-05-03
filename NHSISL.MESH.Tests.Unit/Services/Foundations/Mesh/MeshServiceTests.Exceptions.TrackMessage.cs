// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSISL.MESH.Models.Foundations.Mesh;
using NHSISL.MESH.Models.Foundations.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace NHSISL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationResponseMessages))]
        public async Task ShouldThrowDependencyValidationExceptionIfServerErrorOccursOnTrackMessageAsync(
            HttpResponseMessage dependencyValidationResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomMessage();
            HttpResponseMessage response = dependencyValidationResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(dependencyValidationResponseMessage);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshClientException = new FailedMeshClientException(
                    message: "Mesh client error occurred, contact support.",
                    innerException: httpRequestException);

            failedMeshClientException.AddData("StatusCode", httpRequestException.Message);

            var expectedMeshDependencyValidationException = new MeshDependencyValidationException(
                    message: "Mesh dependency error occurred, contact support.",
                    innerException: failedMeshClientException);

            // when
            ValueTask<Message> sendMessageTask =
                this.meshService.TrackMessageAsync(someMessage.MessageId, authorizationToken);

            MeshDependencyValidationException actualMeshDependencyValidationException =
                await Assert.ThrowsAsync<MeshDependencyValidationException>(sendMessageTask.AsTask);

            // then
            actualMeshDependencyValidationException.Should().BeEquivalentTo(expectedMeshDependencyValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyResponseMessages))]
        public async Task ShouldThrowDependencyExceptionIfServerErrorOccursOnTrackMessageAsync(
            HttpResponseMessage dependencyResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomMessage();
            HttpResponseMessage response = dependencyResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(dependencyResponseMessage);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshServerException = new FailedMeshServerException(
                    message: "Mesh server error occurred, contact support.",
                    innerException: httpRequestException);

            var expectedMeshDependencyException =
                new MeshDependencyException(
                    message: "Mesh dependency error occurred, contact support.",
                    innerException: failedMeshServerException.InnerException as Xeption);

            // when
            ValueTask<Message> sendMessageTask =
                this.meshService.TrackMessageAsync(someMessage.MessageId, authorizationToken);

            MeshDependencyException actualMeshDependencyException =
                await Assert.ThrowsAsync<MeshDependencyException>(sendMessageTask.AsTask);

            // then
            actualMeshDependencyException.Should().BeEquivalentTo(expectedMeshDependencyException);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionIfServiceErrorOccursOnTrackMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomMessage();

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.MovedPermanently)
            {
                ReasonPhrase = GetRandomString()
            };

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(response);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshServiceException = new FailedMeshServiceException(
                message: "Mesh service error occurred, contact support.",
                innerException: httpRequestException);

            var expectedMeshServiceException =
                new MeshServiceException(
                    message: "Mesh service error occurred, contact support.",
                    innerException: failedMeshServiceException as Xeption);

            // when
            ValueTask<Message> sendMessageTask =
                this.meshService.TrackMessageAsync(someMessage.MessageId, authorizationToken);

            MeshServiceException actualMeshServiceException =
                await Assert.ThrowsAsync<MeshServiceException>(sendMessageTask.AsTask);

            // then
            actualMeshServiceException.Should().BeEquivalentTo(expectedMeshServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
