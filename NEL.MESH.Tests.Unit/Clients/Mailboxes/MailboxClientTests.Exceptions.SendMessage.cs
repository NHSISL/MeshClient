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
        public async Task ShouldThrowClientValidationExceptionOnSendMessageIfValidationErrorOccursAsync(
            Xeption orchestrationValidationException)
        {
            // given
            string randomMexTo = GetRandomString();
            string randomMexWorkflowId = GetRandomString();

            var expectedMeshClientValidationException = new MeshClientValidationException(
                innerException: orchestrationValidationException.InnerException as Xeption,
                data: orchestrationValidationException.InnerException.Data);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ThrowsAsync(orchestrationValidationException);

            // when
            ValueTask<Message> sendMessageTask = this.mailboxClient.SendMessageAsync(
                mexTo: randomMexTo,
                mexWorkflowId: randomMexWorkflowId,
                content: new MemoryStream());

            MeshClientValidationException actualMeshClientValidationException =
                await Assert.ThrowsAsync<MeshClientValidationException>(sendMessageTask.AsTask);

            // then
            actualMeshClientValidationException.Should()
                .BeEquivalentTo(expectedMeshClientValidationException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationDependencyExceptions))]
        public async Task ShouldThrowClientDependencyExceptionOnSendMessageIfDependencyErrorOccursAsync(
            Xeption orchestrationDependencyException)
        {
            // given
            string randomMexTo = GetRandomString();
            string randomMexWorkflowId = GetRandomString();

            var expectedMeshClientDependencyException = new MeshClientDependencyException(
                innerException: orchestrationDependencyException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ThrowsAsync(orchestrationDependencyException);

            // when
            ValueTask<Message> sendMessageTask = this.mailboxClient.SendMessageAsync(
                mexTo: randomMexTo,
                mexWorkflowId: randomMexWorkflowId,
                content: new MemoryStream());

            MeshClientDependencyException actualMeshClientDependencyException =
                await Assert.ThrowsAsync<MeshClientDependencyException>(sendMessageTask.AsTask);

            // then
            actualMeshClientDependencyException.Should()
                .BeEquivalentTo(expectedMeshClientDependencyException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OrchestrationServiceExceptions))]
        public async Task ShouldThrowClientServiceExceptionOnSendMessageIfServiceErrorOccursAsync(
            Xeption orchestrationServiceException)
        {
            // given
            string randomMexTo = GetRandomString();
            string randomMexWorkflowId = GetRandomString();

            var expectedMeshClientServiceException = new MeshClientServiceException(
                innerException: orchestrationServiceException.InnerException as Xeption);

            this.meshOrchestrationServiceMock.Setup(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ThrowsAsync(orchestrationServiceException);

            // when
            ValueTask<Message> sendMessageTask = this.mailboxClient.SendMessageAsync(
                mexTo: randomMexTo,
                mexWorkflowId: randomMexWorkflowId,
                content: new MemoryStream());

            MeshClientServiceException actualMeshClientServiceException =
                await Assert.ThrowsAsync<MeshClientServiceException>(sendMessageTask.AsTask);

            // then
            actualMeshClientServiceException.Should()
                .BeEquivalentTo(expectedMeshClientServiceException);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledExceptionOnSendMessageIfCancellationRequestedAsync()
        {
            // given
            string randomMexTo = GetRandomString();
            string randomMexWorkflowId = GetRandomString();
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            this.meshOrchestrationServiceMock.Setup(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    cancellationTokenSource.Token))
                        .ThrowsAsync(new System.OperationCanceledException());

            // when
            ValueTask<Message> sendMessageTask = this.mailboxClient.SendMessageAsync(
                mexTo: randomMexTo,
                mexWorkflowId: randomMexWorkflowId,
                content: new MemoryStream(),
                cancellationToken: cancellationTokenSource.Token);

            // then
            await Assert.ThrowsAsync<System.OperationCanceledException>(sendMessageTask.AsTask);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    cancellationTokenSource.Token),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
