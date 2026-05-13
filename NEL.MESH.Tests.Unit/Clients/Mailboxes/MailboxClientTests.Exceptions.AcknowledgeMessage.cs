// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Clients.Mailboxes
{
    public partial class MailboxClientTests
    {
        [Theory]
        [MemberData(nameof(OrchestrationValidationExceptions))]
        public async Task ShouldThrowClientValidationExceptionOnAcknowledgeMessageIfValidationErrorOccursAsync(
            Xeption orchestrationValidationException)
        {
            // given
            string randomMessageId = GetRandomString();

            var expectedMeshClientValidationException = new MeshClientValidationException(
                innerException: orchestrationValidationException.InnerException as Xeption,
                data: orchestrationValidationException.InnerException.Data);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(randomMessageId, It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationValidationException);

            // when
            ValueTask<bool> acknowledgeMessageTask = this.mailboxClient.AcknowledgeMessageAsync(randomMessageId);

            MeshClientValidationException actualMeshClientValidationException =
                await Assert.ThrowsAsync<MeshClientValidationException>(acknowledgeMessageTask.AsTask);

            // then
            actualMeshClientValidationException.Should()
                .BeEquivalentTo(expectedMeshClientValidationException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(randomMessageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationDependencyExceptions))]
        public async Task ShouldThrowClientDependencyExceptionOnAcknowledgeMessageIfDependencyErrorOccursAsync(
            Xeption orchestrationDependencyException)
        {
            // given
            string randomMessageId = GetRandomString();

            var expectedMeshClientDependencyException = new MeshClientDependencyException(
                innerException: orchestrationDependencyException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(randomMessageId, It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationDependencyException);

            // when
            ValueTask<bool> acknowledgeMessageTask = this.mailboxClient.AcknowledgeMessageAsync(randomMessageId);

            MeshClientDependencyException actualMeshClientDependencyException =
                await Assert.ThrowsAsync<MeshClientDependencyException>(acknowledgeMessageTask.AsTask);

            // then
            actualMeshClientDependencyException.Should()
                .BeEquivalentTo(expectedMeshClientDependencyException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(randomMessageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationServiceExceptions))]
        public async Task ShouldThrowClientServiceExceptionOnAcknowledgeMessageIfServiceErrorOccursAsync(
            Xeption orchestrationServiceException)
        {
            // given
            string randomMessageId = GetRandomString();

            var expectedMeshClientServiceException = new MeshClientServiceException(
                innerException: orchestrationServiceException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(randomMessageId, It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationServiceException);

            // when
            ValueTask<bool> acknowledgeMessageTask = this.mailboxClient.AcknowledgeMessageAsync(randomMessageId);

            MeshClientServiceException actualMeshClientServiceException =
                await Assert.ThrowsAsync<MeshClientServiceException>(acknowledgeMessageTask.AsTask);

            // then
            actualMeshClientServiceException.Should()
                .BeEquivalentTo(expectedMeshClientServiceException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(randomMessageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledExceptionOnAcknowledgeMessageIfCancellationRequestedAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            this.meshOrchestrationServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(randomMessageId, cancellationTokenSource.Token))
                    .ThrowsAsync(new System.OperationCanceledException());

            // when
            ValueTask<bool> acknowledgeMessageTask =
                this.mailboxClient.AcknowledgeMessageAsync(randomMessageId, cancellationTokenSource.Token);

            // then
            await Assert.ThrowsAsync<System.OperationCanceledException>(acknowledgeMessageTask.AsTask);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(randomMessageId, cancellationTokenSource.Token),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
