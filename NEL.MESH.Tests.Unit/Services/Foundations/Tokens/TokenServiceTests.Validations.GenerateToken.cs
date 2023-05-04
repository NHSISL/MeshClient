// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Configurations;
using NEL.MESH.Models.Foundations.Token.Exceptions;
using NEL.MESH.Services.Foundations.Tokens;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Tokens
{
    public partial class TokenServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnGenerateTokenIfArgumentsAreInvalidAsync(string invalidText)
        {
            // given
            string mailboxId = invalidText;
            string password = invalidText;
            string key = invalidText;

            var configuration = new MeshConfiguration
            {
                MailboxId = invalidText,
                Password = invalidText,
                Key = invalidText
            };

            var tokenService = new TokenService(
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                meshConfiguration: configuration);

            Guid identifier = Guid.NewGuid();
            DateTimeOffset randomDateTimeOffset = new DateTime(2022, 3, 24, 12, 00, 00);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(identifier);

            var InvalidTokenArgsException =
                new InvalidTokenArgsException();

            InvalidTokenArgsException.AddData(
                key: "MailboxId",
                values: "Text is required");

            InvalidTokenArgsException.AddData(
                key: "Password",
                values: "Text is required");

            InvalidTokenArgsException.AddData(
                key: "Key",
                values: "Text is required");

            var expectedTokenValidationException =
                 new TokenValidationException(
                    innerException: InvalidTokenArgsException,
                    validationSummary: GetValidationSummary(InvalidTokenArgsException.Data));

            // when
            ValueTask<string> getTokenTask = tokenService.GenerateTokenAsync();

            TokenValidationException actualTokenValidationException =
                await Assert.ThrowsAsync<TokenValidationException>(() =>
                    getTokenTask.AsTask());

            // then
            actualTokenValidationException.Should()
                .BeEquivalentTo(expectedTokenValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
