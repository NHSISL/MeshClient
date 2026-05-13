// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Clients.Mailboxes
{
    public partial class MailboxClientTests
    {
        [Fact]
        public async Task ShouldSendMessageAsync()
        {
            // given
            string randomMexTo = GetRandomString();
            string randomMexWorkflowId = GetRandomString();
            using MemoryStream inputStream = new MemoryStream(new byte[] { 1, 2, 3 });
            Message expectedMessage = new Message { MessageId = GetRandomString() };

            this.meshOrchestrationServiceMock.Setup(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(expectedMessage);

            // when
            Message actualMessage = await this.mailboxClient.SendMessageAsync(
                mexTo: randomMexTo,
                mexWorkflowId: randomMexWorkflowId,
                content: inputStream);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldPassCancellationTokenOnSendMessageAsync()
        {
            // given
            string randomMexTo = GetRandomString();
            string randomMexWorkflowId = GetRandomString();
            using MemoryStream inputStream = new MemoryStream(new byte[] { 1, 2, 3 });
            Message expectedMessage = new Message { MessageId = GetRandomString() };
            using var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken inputCancellationToken = cancellationTokenSource.Token;

            this.meshOrchestrationServiceMock.Setup(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    inputCancellationToken))
                        .ReturnsAsync(expectedMessage);

            // when
            Message actualMessage = await this.mailboxClient.SendMessageAsync(
                mexTo: randomMexTo,
                mexWorkflowId: randomMexWorkflowId,
                content: inputStream,
                cancellationToken: inputCancellationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.SendMessageAsync(
                    It.IsAny<Message>(),
                    It.IsAny<Stream>(),
                    inputCancellationToken),
                        Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
