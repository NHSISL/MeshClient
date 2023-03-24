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
            string randomMailboxId = GetRandomString(wordCount: 1);
            string mailboxId = randomMailboxId;
            string randomPassword = GetRandomString();
            string password = randomPassword;
            string randomKey = GetRandomString();
            string key = randomKey;
            int nonce_count = GetRandomNumber();
            string nonce = this.identifierBrokerMock.Object.GetIdentifier().ToString();
            string timeStamp = this.dateTimeBrokerMock.Object.GetCurrentDateTimeOffset().ToString("yyyyMMddHHmm");
            string stringToHash = $"{mailboxId}:{nonce}:{nonce_count}:{password}:{timeStamp}";
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomIdentifier = Guid.NewGuid();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomIdentifier);

            string expectedToken = HashStringSha256(stringToHash, key);

            // when
            string actualToken = await this.tokenService.GenerateTokenAsync(mailboxId, password, key);

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
