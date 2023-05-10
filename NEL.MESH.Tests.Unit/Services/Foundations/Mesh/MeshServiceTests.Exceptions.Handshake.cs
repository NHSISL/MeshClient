// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Chunking.Exceptions;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationResponseMessages))]
        public async Task ShouldThrowDependencyValidationExceptionIfClientErrorOccursOnHandshakeAsync(
            HttpResponseMessage dependencyValidationResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            HttpResponseMessage response = dependencyValidationResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.HandshakeAsync(authorizationToken))
                    .ReturnsAsync(dependencyValidationResponseMessage);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshClientException =
                new FailedMeshClientException(httpRequestException);

            var expectedMeshDependencyValidationException =
                new MeshDependencyValidationException(failedMeshClientException.InnerException as Xeption);

            // when
            ValueTask<bool> handshakeTask =
                this.meshService.HandshakeAsync(authorizationToken);

            MeshDependencyValidationException actualMeshDependencyValidationException =
                await Assert.ThrowsAsync<MeshDependencyValidationException>(handshakeTask.AsTask);

            // then
            actualMeshDependencyValidationException.Should().BeEquivalentTo(expectedMeshDependencyValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.HandshakeAsync(authorizationToken),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyResponseMessages))]
        public async Task ShouldThrowDependencyExceptionIfServerErrorOccursOnHandshakeAsync(
            HttpResponseMessage dependencyResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            HttpResponseMessage response = dependencyResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.HandshakeAsync(authorizationToken))
                    .ReturnsAsync(dependencyResponseMessage);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshServerException =
                new FailedMeshServerException(httpRequestException);

            var expectedMeshDependencyException =
                new MeshDependencyException(failedMeshServerException.InnerException as Xeption);

            // when
            ValueTask<bool> handshakeTask =
                this.meshService.HandshakeAsync(authorizationToken);

            MeshDependencyException actualMeshDependencyException =
                await Assert.ThrowsAsync<MeshDependencyException>(handshakeTask.AsTask);

            // then
            actualMeshDependencyException.Should().BeEquivalentTo(expectedMeshDependencyException);

            this.meshBrokerMock.Verify(broker =>
                broker.HandshakeAsync(authorizationToken),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionIfServiceErrorOccursOnHandshakeAsync()
        {
            // given
            string authorizationToken = GetRandomString();

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.MovedPermanently)
            {
                ReasonPhrase = GetRandomString()
            };

            this.meshBrokerMock.Setup(broker =>
                broker.HandshakeAsync(authorizationToken))
                    .ReturnsAsync(response);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshServiceException =
                new FailedMeshServiceException(httpRequestException);

            var expectedMeshServiceException =
                new MeshServiceException(failedMeshServiceException as Xeption);

            // when
            ValueTask<bool> handshakeTask =
                this.meshService.HandshakeAsync(authorizationToken);

            MeshServiceException actualMeshServiceException =
                await Assert.ThrowsAsync<MeshServiceException>(handshakeTask.AsTask);

            // then
            actualMeshServiceException.Should().BeEquivalentTo(expectedMeshServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.HandshakeAsync(authorizationToken),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
