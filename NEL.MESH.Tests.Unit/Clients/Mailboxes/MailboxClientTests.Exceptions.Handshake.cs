// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Clients.Mailboxes
{
    public partial class MailboxClientTests
    {
        [Theory]
        [MemberData(nameof(OrchestrationValidationExceptions))]
        public async Task ShouldThrowClientValidationExceptionOnHandshakeIfValidationErrorOccursAsync(
            Xeption orchestrationValidationException)
        {
            // given
            var expectedMeshClientValidationException = new MeshClientValidationException(
                innerException: orchestrationValidationException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.HandshakeAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationValidationException);

            // when
            ValueTask<bool> handshakeTask = this.mailboxClient.HandshakeAsync();

            MeshClientValidationException actualMeshClientValidationException =
                await Assert.ThrowsAsync<MeshClientValidationException>(handshakeTask.AsTask);

            // then
            actualMeshClientValidationException.Should()
                .BeEquivalentTo(expectedMeshClientValidationException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.HandshakeAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationDependencyExceptions))]
        public async Task ShouldThrowClientDependencyExceptionOnHandshakeIfDependencyErrorOccursAsync(
            Xeption orchestrationDependencyException)
        {
            // given
            var expectedMeshClientDependencyException = new MeshClientDependencyException(
                innerException: orchestrationDependencyException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.HandshakeAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationDependencyException);

            // when
            ValueTask<bool> handshakeTask = this.mailboxClient.HandshakeAsync();

            MeshClientDependencyException actualMeshClientDependencyException =
                await Assert.ThrowsAsync<MeshClientDependencyException>(handshakeTask.AsTask);

            // then
            actualMeshClientDependencyException.Should()
                .BeEquivalentTo(expectedMeshClientDependencyException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.HandshakeAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationServiceExceptions))]
        public async Task ShouldThrowClientServiceExceptionOnHandshakeIfServiceErrorOccursAsync(
            Xeption orchestrationServiceException)
        {
            // given
            var expectedMeshClientServiceException = new MeshClientServiceException(
                innerException: orchestrationServiceException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.HandshakeAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationServiceException);

            // when
            ValueTask<bool> handshakeTask = this.mailboxClient.HandshakeAsync();

            MeshClientServiceException actualMeshClientServiceException =
                await Assert.ThrowsAsync<MeshClientServiceException>(handshakeTask.AsTask);

            // then
            actualMeshClientServiceException.Should()
                .BeEquivalentTo(expectedMeshClientServiceException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.HandshakeAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledExceptionOnHandshakeIfCancellationRequestedAsync()
        {
            // given
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            this.meshOrchestrationServiceMock.Setup(service =>
                service.HandshakeAsync(cancellationTokenSource.Token))
                    .ThrowsAsync(new System.OperationCanceledException());

            // when
            ValueTask<bool> handshakeTask =
                this.mailboxClient.HandshakeAsync(cancellationTokenSource.Token);

            // then
            await Assert.ThrowsAsync<System.OperationCanceledException>(handshakeTask.AsTask);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.HandshakeAsync(cancellationTokenSource.Token),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
