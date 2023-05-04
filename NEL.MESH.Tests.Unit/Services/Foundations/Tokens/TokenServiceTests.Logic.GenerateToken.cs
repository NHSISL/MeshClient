// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Tokens
{
    public partial class TokenServiceTests
    {
        [Fact]
        public async Task ShouldGenerateTokenAsync()
        {
            // given
            Guid identifier = Guid.NewGuid();
            DateTimeOffset randomDateTimeOffset = new DateTime(2022, 3, 24, 12, 00, 00);
            string timeStamp = randomDateTimeOffset.ToString("yyyyMMddHHmm");
            string nonce = identifier.ToString();
            int nonce_count = 0;

            string inputStringToHash =
                $"{this.meshConfiguration.MailboxId}:"
                + $"{nonce}:"
                + $"{nonce_count}:"
                + $"{this.meshConfiguration.Password}:"
                + $"{timeStamp}";

            string inputSharedKey = HashStringSha256(inputStringToHash, this.meshConfiguration.Key);

            string expectedToken =
                $"NHSMESH {this.meshConfiguration.MailboxId}:"
                + $"{nonce}:{nonce_count}:{timeStamp}:{inputSharedKey}";

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(identifier);

            // when
            string actualToken = await this.tokenService.GenerateTokenAsync();

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
