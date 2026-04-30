// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace NEL.MESH.Tests.Unit.Clients.Mailboxes
{
    public partial class MailboxClientTests
    {
        [Fact]
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            bool expectedResult = true;

            this.meshOrchestrationServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(randomMessageId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedResult);

            // when
            bool actualResult = await this.mailboxClient.AcknowledgeMessageAsync(randomMessageId);

            // then
            actualResult.Should().BeTrue();

            this.meshOrchestrationServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(randomMessageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldPassCancellationTokenOnAcknowledgeMessageAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            bool expectedResult = true;
            using var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken inputCancellationToken = cancellationTokenSource.Token;

            this.meshOrchestrationServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(randomMessageId, inputCancellationToken))
                    .ReturnsAsync(expectedResult);

            // when
            bool actualResult =
                await this.mailboxClient.AcknowledgeMessageAsync(randomMessageId, inputCancellationToken);

            // then
            actualResult.Should().BeTrue();

            this.meshOrchestrationServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(randomMessageId, inputCancellationToken),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
