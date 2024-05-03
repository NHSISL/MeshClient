// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSISL.MESH.Models.Configurations;
using NHSISL.MESH.Models.Foundations.Tokens.Exceptions;
using Xeptions;
using Xunit;

namespace NHSISL.MESH.Tests.Unit.Services.Foundations.Tokens
{
    public partial class TokenServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionIfServiceErrorOccursOnGenerateTokenAsync()
        {
            // given
            string mailboxId = GetRandomString();
            string password = GetRandomString();
            string key = GetRandomString();

            MeshConfiguration meshConfiguration = new MeshConfiguration
            {
                MailboxId = mailboxId,
                Password = password,
                Key = key
            };

            this.meshBrokerMock.Setup(broker =>
                broker.MeshConfiguration)
                    .Returns(meshConfiguration);

            string someMessage = GetRandomString();
            Exception someException = new Exception(message: someMessage);

            var failedTokenServiceException = new FailedTokenServiceException(
                message: "Token service error occurred, contact support.",
                innerException: someException,
                data: someException.Data);

            var expectedTokenServiceException = new TokenServiceException(
                message: "Token service error occurred, contact support.",
                failedTokenServiceException as Xeption);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Throws(someException);

            // when
            ValueTask<string> handshakeTask =
                this.tokenService.GenerateTokenAsync();

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
