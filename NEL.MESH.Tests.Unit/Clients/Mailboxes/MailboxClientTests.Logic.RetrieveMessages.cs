// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldRetrieveMessagesAsync()
        {
            // given
            List<string> expectedMessageIds = new List<string> { GetRandomString(), GetRandomString() };

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedMessageIds);

            // when
            List<string> actualMessageIds = await this.mailboxClient.RetrieveMessagesAsync();

            // then
            actualMessageIds.Should().BeEquivalentTo(expectedMessageIds);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldPassCancellationTokenOnRetrieveMessagesAsync()
        {
            // given
            List<string> expectedMessageIds = new List<string> { GetRandomString(), GetRandomString() };
            using var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken inputCancellationToken = cancellationTokenSource.Token;

            this.meshOrchestrationServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(inputCancellationToken))
                    .ReturnsAsync(expectedMessageIds);

            // when
            List<string> actualMessageIds =
                await this.mailboxClient.RetrieveMessagesAsync(inputCancellationToken);

            // then
            actualMessageIds.Should().BeEquivalentTo(expectedMessageIds);

            this.meshOrchestrationServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(inputCancellationToken),
                    Times.Once);

            this.meshOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
