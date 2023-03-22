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
            Message someMessage = CreateRandomSendMessage();
            HttpResponseMessage response = dependencyValidationResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessageAsync(It.IsAny<string>()))
                    .ReturnsAsync(dependencyValidationResponseMessage);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshClientException =
                new FailedMeshClientException(httpRequestException);

            var expectedMeshDependencyValidationException =
                new MeshDependencyValidationException(failedMeshClientException.InnerException as Xeption);

            // when
            ValueTask<Message> getMessageTask =
                this.meshService.GetMessageAsync(someMessage.MessageId);

            MeshDependencyValidationException actualMeshDependencyValidationException =
                await Assert.ThrowsAsync<MeshDependencyValidationException>(getMessageTask.AsTask);

            // then
            actualMeshDependencyValidationException.Should().BeEquivalentTo(expectedMeshDependencyValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyResponseMessages))]
        public async Task ShouldThrowDependencyExceptionIfServerErrorOccursOnGetMessageAsync(
            HttpResponseMessage dependencyResponseMessage)
        {
            // given
            Message someMessage = CreateRandomSendMessage();
            HttpResponseMessage response = dependencyResponseMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessageAsync(
                    It.IsAny<string>()))
                    .ReturnsAsync(dependencyResponseMessage);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshServerException =
                new FailedMeshServerException(httpRequestException);

            var expectedMeshDependencyException =
                new MeshDependencyException(failedMeshServerException.InnerException as Xeption);

            // when
            ValueTask<Message> GetMessageTask =
                this.meshService.GetMessageAsync(someMessage.MessageId);

            MeshDependencyException actualMeshDependencyException =
                await Assert.ThrowsAsync<MeshDependencyException>(GetMessageTask.AsTask);

            // then
            actualMeshDependencyException.Should().BeEquivalentTo(expectedMeshDependencyException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(
                    It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionIfServiceErrorOccursOnGetMessageAsync()
        {
            // given
            Message someMessage = CreateRandomSendMessage();

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.MovedPermanently)
            {
                ReasonPhrase = GetRandomString()
            };

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessageAsync(
                    It.IsAny<string>()))
                    .ReturnsAsync(response);

            var httpRequestException =
                new HttpRequestException($"{(int)response.StatusCode} - {response.ReasonPhrase}");

            var failedMeshServiceException =
                new FailedMeshServiceException(httpRequestException);

            var expectedMeshServiceException =
                new MeshServiceException(failedMeshServiceException as Xeption);

            // when
            ValueTask<Message> getMessageTask =
                this.meshService.GetMessageAsync(someMessage.MessageId);

            MeshServiceException actualMeshServiceException =
                await Assert.ThrowsAsync<MeshServiceException>(getMessageTask.AsTask);

            // then
            actualMeshServiceException.Should().BeEquivalentTo(expectedMeshServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(
                    It.IsAny<string>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
