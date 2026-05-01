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
        public async Task ShouldRetrieveMessageAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            using MemoryStream outputStream = new MemoryStream();
            Message expectedMessage = new Message { MessageId = randomMessageId };

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessageAsync(randomMessageId, outputStream, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedMessage);

            // when
            Message actualMessage =
                await this.mailboxClient.RetrieveMessageAsync(randomMessageId, outputStream);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessageAsync(randomMessageId, outputStream, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldPassCancellationTokenOnRetrieveMessageAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            using MemoryStream outputStream = new MemoryStream();
            Message expectedMessage = new Message { MessageId = randomMessageId };
            using var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken inputCancellationToken = cancellationTokenSource.Token;

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessageAsync(randomMessageId, outputStream, inputCancellationToken))
                    .ReturnsAsync(expectedMessage);

            // when
            Message actualMessage =
                await this.mailboxClient.RetrieveMessageAsync(randomMessageId, outputStream, inputCancellationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessageAsync(randomMessageId, outputStream, inputCancellationToken),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
