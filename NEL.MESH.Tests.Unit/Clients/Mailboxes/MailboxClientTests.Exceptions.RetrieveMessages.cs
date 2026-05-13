// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowClientValidationExceptionOnRetrieveMessagesIfValidationErrorOccursAsync(
            Xeption orchestrationValidationException)
        {
            // given
            var expectedMeshClientValidationException = new MeshClientValidationException(
                innerException: orchestrationValidationException.InnerException as Xeption,
                data: orchestrationValidationException.InnerException.Data);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationValidationException);

            // when
            ValueTask<List<string>> retrieveMessagesTask = this.mailboxClient.RetrieveMessagesAsync();

            MeshClientValidationException actualMeshClientValidationException =
                await Assert.ThrowsAsync<MeshClientValidationException>(retrieveMessagesTask.AsTask);

            // then
            actualMeshClientValidationException.Should()
                .BeEquivalentTo(expectedMeshClientValidationException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationDependencyExceptions))]
        public async Task ShouldThrowClientDependencyExceptionOnRetrieveMessagesIfDependencyErrorOccursAsync(
            Xeption orchestrationDependencyException)
        {
            // given
            var expectedMeshClientDependencyException = new MeshClientDependencyException(
                innerException: orchestrationDependencyException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationDependencyException);

            // when
            ValueTask<List<string>> retrieveMessagesTask = this.mailboxClient.RetrieveMessagesAsync();

            MeshClientDependencyException actualMeshClientDependencyException =
                await Assert.ThrowsAsync<MeshClientDependencyException>(retrieveMessagesTask.AsTask);

            // then
            actualMeshClientDependencyException.Should()
                .BeEquivalentTo(expectedMeshClientDependencyException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationServiceExceptions))]
        public async Task ShouldThrowClientServiceExceptionOnRetrieveMessagesIfServiceErrorOccursAsync(
            Xeption orchestrationServiceException)
        {
            // given
            var expectedMeshClientServiceException = new MeshClientServiceException(
                innerException: orchestrationServiceException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(orchestrationServiceException);

            // when
            ValueTask<List<string>> retrieveMessagesTask = this.mailboxClient.RetrieveMessagesAsync();

            MeshClientServiceException actualMeshClientServiceException =
                await Assert.ThrowsAsync<MeshClientServiceException>(retrieveMessagesTask.AsTask);

            // then
            actualMeshClientServiceException.Should()
                .BeEquivalentTo(expectedMeshClientServiceException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledExceptionOnRetrieveMessagesIfCancellationRequestedAsync()
        {
            // given
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(cancellationTokenSource.Token))
                    .ThrowsAsync(new System.OperationCanceledException());

            // when
            ValueTask<List<string>> retrieveMessagesTask =
                this.mailboxClient.RetrieveMessagesAsync(cancellationTokenSource.Token);

            // then
            await Assert.ThrowsAsync<System.OperationCanceledException>(retrieveMessagesTask.AsTask);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(cancellationTokenSource.Token),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
