// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowDependencyValidationExceptionIfServerErrorOccursOnRetrieveMessagesAsync(
            HttpResponseMessage dependencyValidationResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomSendMessage();
            HttpResponseMessage response = dependencyValidationResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessagesAsync(
                    It.IsAny<string>()))
                        .ReturnsAsync(response);

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
            ValueTask<List<string>> getMessagesTask =
                this.meshService.RetrieveMessagesAsync(authorizationToken);

            MeshDependencyValidationException actualMeshDependencyValidationException =
                await Assert.ThrowsAsync<MeshDependencyValidationException>(getMessagesTask.AsTask);

            // then
            actualMeshDependencyValidationException.Should().BeEquivalentTo(expectedMeshDependencyValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessagesAsync(It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyResponseMessages))]
        public async Task ShouldThrowDependencyExceptionIfServerErrorOccursOnRetrieveMessagesAsync(
            HttpResponseMessage dependencyResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomSendMessage();
            HttpResponseMessage response = dependencyResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessagesAsync(
                    It.IsAny<string>()))
                        .ReturnsAsync(response);

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
            ValueTask<List<string>> getMessagesTask =
                this.meshService.RetrieveMessagesAsync(authorizationToken);

            MeshDependencyException actualMeshDependencyException =
                await Assert.ThrowsAsync<MeshDependencyException>(getMessagesTask.AsTask);

            // then
            actualMeshDependencyException.Should().BeEquivalentTo(expectedMeshDependencyException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessagesAsync(It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionIfServiceErrorOccursOnRetrieveMessagesAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomSendMessage();

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.MovedPermanently)
            {
                ReasonPhrase = GetRandomString()
            };

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessagesAsync(
                    It.IsAny<string>()))
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
            ValueTask<List<string>> getMessagesTask =
                this.meshService.RetrieveMessagesAsync(authorizationToken);

            MeshServiceException actualMeshServiceException =
                await Assert.ThrowsAsync<MeshServiceException>(getMessagesTask.AsTask);

            // then
            actualMeshServiceException.Should().BeEquivalentTo(expectedMeshServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessagesAsync(It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
