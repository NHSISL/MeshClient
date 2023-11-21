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
        public async Task ShouldThrowDependencyValidationExceptionIfServerErrorOccursOnGetMessageAsync(
            HttpResponseMessage dependencyValidationResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomSendMessage();
            HttpResponseMessage response = dependencyValidationResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(dependencyValidationResponseMessage);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshClientException =
                new FailedMeshClientException(innerException: httpRequestException);

            failedMeshClientException.AddData("StatusCode", httpRequestException.Message);

            var expectedMeshDependencyValidationException =
                new MeshDependencyValidationException(innerException: failedMeshClientException);

            // when
            ValueTask<Message> getMessageTask =
                this.meshService.RetrieveMessageAsync(someMessage.MessageId, authorizationToken);

            MeshDependencyValidationException actualMeshDependencyValidationException =
                await Assert.ThrowsAsync<MeshDependencyValidationException>(getMessageTask.AsTask);

            // then
            actualMeshDependencyValidationException.Should().BeEquivalentTo(expectedMeshDependencyValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyResponseMessages))]
        public async Task ShouldThrowDependencyExceptionIfServerErrorOccursOnRetrieveMessageAsync(
            HttpResponseMessage dependencyResponseMessage)
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomSendMessage();
            HttpResponseMessage response = dependencyResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessageAsync(
                    It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(dependencyResponseMessage);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshServerException =
                new FailedMeshServerException(httpRequestException);

            var expectedMeshDependencyException =
                new MeshDependencyException(failedMeshServerException.InnerException as Xeption);

            // when
            ValueTask<Message> GetMessageTask =
                this.meshService.RetrieveMessageAsync(someMessage.MessageId, authorizationToken);

            MeshDependencyException actualMeshDependencyException =
                await Assert.ThrowsAsync<MeshDependencyException>(GetMessageTask.AsTask);

            // then
            actualMeshDependencyException.Should().BeEquivalentTo(expectedMeshDependencyException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(
                    It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionIfServiceErrorOccursOnGetMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message someMessage = CreateRandomSendMessage();

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.MovedPermanently)
            {
                ReasonPhrase = GetRandomString()
            };

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessageAsync(
                    It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(response);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshServiceException =
                new FailedMeshServiceException(httpRequestException);

            var expectedMeshServiceException =
                new MeshServiceException(failedMeshServiceException as Xeption);

            // when
            ValueTask<Message> getMessageTask =
                this.meshService.RetrieveMessageAsync(someMessage.MessageId, authorizationToken);

            MeshServiceException actualMeshServiceException =
                await Assert.ThrowsAsync<MeshServiceException>(getMessageTask.AsTask);

            // then
            actualMeshServiceException.Should().BeEquivalentTo(expectedMeshServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
