// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
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
        public async Task ShouldThrowClientValidationExceptionOnRetrieveMessageIfValidationErrorOccursAsync(
            Xeption orchestrationValidationException)
        {
            // given
            string randomMessageId = GetRandomString();
            using MemoryStream outputStream = new MemoryStream();

            var expectedMeshClientValidationException = new MeshClientValidationException(
                innerException: orchestrationValidationException.InnerException as Xeption,
                data: orchestrationValidationException.InnerException.Data);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessageAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ThrowsAsync(orchestrationValidationException);

            // when
            ValueTask<Message> retrieveMessageTask =
                this.mailboxClient.RetrieveMessageAsync(randomMessageId, outputStream);

            MeshClientValidationException actualMeshClientValidationException =
                await Assert.ThrowsAsync<MeshClientValidationException>(retrieveMessageTask.AsTask);

            // then
            actualMeshClientValidationException.Should()
                .BeEquivalentTo(expectedMeshClientValidationException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessageAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationDependencyExceptions))]
        public async Task ShouldThrowClientDependencyExceptionOnRetrieveMessageIfDependencyErrorOccursAsync(
            Xeption orchestrationDependencyException)
        {
            // given
            string randomMessageId = GetRandomString();
            using MemoryStream outputStream = new MemoryStream();

            var expectedMeshClientDependencyException = new MeshClientDependencyException(
                innerException: orchestrationDependencyException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessageAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ThrowsAsync(orchestrationDependencyException);

            // when
            ValueTask<Message> retrieveMessageTask =
                this.mailboxClient.RetrieveMessageAsync(randomMessageId, outputStream);

            MeshClientDependencyException actualMeshClientDependencyException =
                await Assert.ThrowsAsync<MeshClientDependencyException>(retrieveMessageTask.AsTask);

            // then
            actualMeshClientDependencyException.Should()
                .BeEquivalentTo(expectedMeshClientDependencyException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessageAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationServiceExceptions))]
        public async Task ShouldThrowClientServiceExceptionOnRetrieveMessageIfServiceErrorOccursAsync(
            Xeption orchestrationServiceException)
        {
            // given
            string randomMessageId = GetRandomString();
            using MemoryStream outputStream = new MemoryStream();

            var expectedMeshClientServiceException = new MeshClientServiceException(
                innerException: orchestrationServiceException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessageAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ThrowsAsync(orchestrationServiceException);

            // when
            ValueTask<Message> retrieveMessageTask =
                this.mailboxClient.RetrieveMessageAsync(randomMessageId, outputStream);

            MeshClientServiceException actualMeshClientServiceException =
                await Assert.ThrowsAsync<MeshClientServiceException>(retrieveMessageTask.AsTask);

            // then
            actualMeshClientServiceException.Should()
                .BeEquivalentTo(expectedMeshClientServiceException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessageAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledExceptionOnRetrieveMessageIfCancellationRequestedAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            using MemoryStream outputStream = new MemoryStream();
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessageAsync(
                    randomMessageId,
                    outputStream,
                    cancellationTokenSource.Token))
                        .ThrowsAsync(new System.OperationCanceledException());

            // when
            ValueTask<Message> retrieveMessageTask =
                this.mailboxClient.RetrieveMessageAsync(
                    randomMessageId,
                    outputStream,
                    cancellationTokenSource.Token);

            // then
            await Assert.ThrowsAsync<System.OperationCanceledException>(retrieveMessageTask.AsTask);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessageAsync(
                    randomMessageId,
                    outputStream,
                    cancellationTokenSource.Token),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
