// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Token.Exceptions;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Tokens
{
    public partial class TokenServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionIfServiceErrorOccursOnGenerateTokenAsync()
        {
            // given
            string someMessage = GetRandomString();
            string someMailbox = GetRandomString();
            string somePassword = GetRandomString();
            string someKey = GetRandomString();
            Exception someException = new Exception(someMessage);

            var failedTokenServiceException =
                new FailedTokenServiceException(someException);

            var expectedTokenServiceException =
                new TokenServiceException(failedTokenServiceException as Xeption);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Throws(someException);

            // when
            ValueTask<string> handshakeTask =
                this.tokenService.GenerateTokenAsync(
                    mailboxId: someMailbox,
                    password: somePassword,
                    key: someKey);

            TokenServiceException actualTokenServiceException =
                await Assert.ThrowsAsync<TokenServiceException>(handshakeTask.AsTask);

            // then
            actualTokenServiceException.Should().BeEquivalentTo(expectedTokenServiceException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once);

            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
