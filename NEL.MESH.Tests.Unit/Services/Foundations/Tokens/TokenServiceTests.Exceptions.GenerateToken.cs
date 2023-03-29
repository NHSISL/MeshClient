// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Configurations;
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
