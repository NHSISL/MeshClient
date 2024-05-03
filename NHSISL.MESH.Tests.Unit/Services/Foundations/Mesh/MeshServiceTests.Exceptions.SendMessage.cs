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
        public async Task ShouldThrowDependencyValidationExceptionIfServerErrorOccursOnSendMessageAsync(
            HttpResponseMessage dependencyValidationResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomSendMessage();
            HttpResponseMessage response = dependencyValidationResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()))
                        .ReturnsAsync(dependencyValidationResponseMessage);

            var httpRequestException =
                new HttpRequestException(
                    $"{(int)response.StatusCode} - {response.ReasonPhrase}",
                    null,
                    response.StatusCode);

            var failedMeshClientException = new FailedMeshClientException(
                    message: "Mesh client error occurred, contact support.",
                    innerException: httpRequestException);

            failedMeshClientException.AddData("StatusCode", httpRequestException.Message);

            var expectedMeshDependencyValidationException = new MeshDependencyValidationException(
                    message: "Mesh dependency error occurred, contact support.",
                    innerException: failedMeshClientException);

            // when
            ValueTask<Message> sendMessageTask =
                this.meshService.SendMessageAsync(someMessage, authorizationToken);

            MeshDependencyValidationException actualMeshDependencyValidationException =
                await Assert.ThrowsAsync<MeshDependencyValidationException>(sendMessageTask.AsTask);

            // then
            actualMeshDependencyValidationException.Should().BeEquivalentTo(expectedMeshDependencyValidationException);

            this.meshBrokerMock.Verify(broker =>
               broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyResponseMessages))]
        public async Task ShouldThrowDependencyExceptionIfServerErrorOccursOnSendMessageAsync(
            HttpResponseMessage dependencyResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomSendMessage();
            HttpResponseMessage response = dependencyResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()))
                    .ReturnsAsync(dependencyResponseMessage);

            var httpRequestException =
                new HttpRequestException(
                    $"{(int)response.StatusCode} - {response.ReasonPhrase}",
                    null,
                    response.StatusCode);

            var failedMeshServerException = new FailedMeshServerException(
                    message: "Mesh server error occurred, contact support.",
                    innerException: httpRequestException);

            var expectedMeshDependencyException =
                new MeshDependencyException(
                    message: "Mesh dependency error occurred, contact support.",
                    innerException: failedMeshServerException.InnerException as Xeption);

            // when
            ValueTask<Message> sendMessageTask =
                this.meshService.SendMessageAsync(someMessage, authorizationToken);

            MeshDependencyException actualMeshDependencyException =
                await Assert.ThrowsAsync<MeshDependencyException>(sendMessageTask.AsTask);

            // then
            actualMeshDependencyException.Should().BeEquivalentTo(expectedMeshDependencyException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionIfServiceErrorOccursOnSendMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomSendMessage();

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.MovedPermanently)
            {
                ReasonPhrase = GetRandomString()
            };

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()))
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
                this.meshService.SendMessageAsync(someMessage, authorizationToken);

            MeshServiceException actualMeshServiceException =
                await Assert.ThrowsAsync<MeshServiceException>(sendMessageTask.AsTask);

            // then
            actualMeshServiceException.Should().BeEquivalentTo(expectedMeshServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
