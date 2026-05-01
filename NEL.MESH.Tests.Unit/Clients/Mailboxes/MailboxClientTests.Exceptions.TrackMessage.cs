// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Clients.Mailboxes
{
    public partial class MailboxClientTests
    {
        [Theory]
        [MemberData(nameof(OrchestrationValidationExceptions))]
        public async Task ShouldThrowClientValidationExceptionOnTrackMessageIfValidationErrorOccursAsync(
            Xeption orchestrationValidationException)
        {
            // given
            string randomMessageId = GetRandomString();

            var expectedMeshClientValidationException = new MeshClientValidationException(
                innerException: orchestrationValidationException.InnerException as Xeption,
                data: orchestrationValidationException.InnerException.Data);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.TrackMessageAsync(randomMessageId, It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationValidationException);

            // when
            ValueTask<Message> trackMessageTask = this.mailboxClient.TrackMessageAsync(randomMessageId);

            MeshClientValidationException actualMeshClientValidationException =
                await Assert.ThrowsAsync<MeshClientValidationException>(trackMessageTask.AsTask);

            // then
            actualMeshClientValidationException.Should()
                .BeEquivalentTo(expectedMeshClientValidationException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.TrackMessageAsync(randomMessageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationDependencyExceptions))]
        public async Task ShouldThrowClientDependencyExceptionOnTrackMessageIfDependencyErrorOccursAsync(
            Xeption orchestrationDependencyException)
        {
            // given
            string randomMessageId = GetRandomString();

            var expectedMeshClientDependencyException = new MeshClientDependencyException(
                innerException: orchestrationDependencyException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.TrackMessageAsync(randomMessageId, It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationDependencyException);

            // when
            ValueTask<Message> trackMessageTask = this.mailboxClient.TrackMessageAsync(randomMessageId);

            MeshClientDependencyException actualMeshClientDependencyException =
                await Assert.ThrowsAsync<MeshClientDependencyException>(trackMessageTask.AsTask);

            // then
            actualMeshClientDependencyException.Should()
                .BeEquivalentTo(expectedMeshClientDependencyException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.TrackMessageAsync(randomMessageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationServiceExceptions))]
        public async Task ShouldThrowClientServiceExceptionOnTrackMessageIfServiceErrorOccursAsync(
            Xeption orchestrationServiceException)
        {
            // given
            string randomMessageId = GetRandomString();

            var expectedMeshClientServiceException = new MeshClientServiceException(
                innerException: orchestrationServiceException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.TrackMessageAsync(randomMessageId, It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationServiceException);

            // when
            ValueTask<Message> trackMessageTask = this.mailboxClient.TrackMessageAsync(randomMessageId);

            MeshClientServiceException actualMeshClientServiceException =
                await Assert.ThrowsAsync<MeshClientServiceException>(trackMessageTask.AsTask);

            // then
            actualMeshClientServiceException.Should()
                .BeEquivalentTo(expectedMeshClientServiceException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.TrackMessageAsync(randomMessageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledExceptionOnTrackMessageIfCancellationRequestedAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            this.meshOrchestrationServiceMock.Setup(service =>
                service.TrackMessageAsync(randomMessageId, cancellationTokenSource.Token))
                    .ThrowsAsync(new System.OperationCanceledException());

            // when
            ValueTask<Message> trackMessageTask =
                this.mailboxClient.TrackMessageAsync(randomMessageId, cancellationTokenSource.Token);

            // then
            await Assert.ThrowsAsync<System.OperationCanceledException>(trackMessageTask.AsTask);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.TrackMessageAsync(randomMessageId, cancellationTokenSource.Token),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
