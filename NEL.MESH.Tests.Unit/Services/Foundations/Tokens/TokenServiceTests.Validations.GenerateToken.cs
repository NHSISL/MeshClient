﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Token.Exceptions;
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
            string inputKey = invalidText;
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
            ValueTask<string> getTokenTask =
                this.tokenService.GenerateTokenAsync(mailboxId, password, inputKey);

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