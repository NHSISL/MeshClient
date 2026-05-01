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
        public async Task ShouldDoHandshakeAsync()
        {
            // given
            bool expectedResult = true;

            this.meshOrchestrationServiceMock.Setup(service =>
                service.HandshakeAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedResult);

            // when
            bool actualResult = await this.mailboxClient.HandshakeAsync();

            // then
            actualResult.Should().BeTrue();

            this.meshOrchestrationServiceMock.Verify(service =>
                service.HandshakeAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldPassCancellationTokenOnHandshakeAsync()
        {
            // given
            bool expectedResult = true;
            using var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken inputCancellationToken = cancellationTokenSource.Token;

            this.meshOrchestrationServiceMock.Setup(service =>
                service.HandshakeAsync(inputCancellationToken))
                    .ReturnsAsync(expectedResult);

            // when
            bool actualResult = await this.mailboxClient.HandshakeAsync(inputCancellationToken);

            // then
            actualResult.Should().BeTrue();

            this.meshOrchestrationServiceMock.Verify(service =>
                service.HandshakeAsync(inputCancellationToken),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
