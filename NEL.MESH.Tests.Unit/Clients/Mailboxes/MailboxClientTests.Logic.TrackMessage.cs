// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldTrackMessageAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            Message expectedMessage = new Message { MessageId = randomMessageId };

            this.meshOrchestrationServiceMock.Setup(service =>
                service.TrackMessageAsync(randomMessageId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedMessage);

            // when
            Message actualMessage = await this.mailboxClient.TrackMessageAsync(randomMessageId);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.TrackMessageAsync(randomMessageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldPassCancellationTokenOnTrackMessageAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            Message expectedMessage = new Message { MessageId = randomMessageId };
            using var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken inputCancellationToken = cancellationTokenSource.Token;

            this.meshOrchestrationServiceMock.Setup(service =>
                service.TrackMessageAsync(randomMessageId, inputCancellationToken))
                    .ReturnsAsync(expectedMessage);

            // when
            Message actualMessage =
                await this.mailboxClient.TrackMessageAsync(randomMessageId, inputCancellationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.TrackMessageAsync(randomMessageId, inputCancellationToken),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
