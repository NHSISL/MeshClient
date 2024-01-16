// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnGetMessageIfMessageIdIsNullOrEmptyAsync(string invalidText)
        {
            // given
            string invalidAuthorizationToken = invalidText;
            Message randomMessage = CreateRandomMessage();
            randomMessage.MessageId = invalidText;
            Message inputMessage = randomMessage;

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessageForSendMessage(
                inputMessage,
                new Dictionary<string, List<string>>());

            this.meshBrokerMock.Setup(broker =>
              broker.GetMessageAsync(inputMessage.MessageId, invalidAuthorizationToken))
                  .ReturnsAsync(responseMessage);

            var invalidMeshArgsException = new InvalidArgumentsMeshException(
                    message: "Invalid MESH argument validation errors occurred, " +
                    "please correct the errors and try again.");

            invalidMeshArgsException.AddData(
               key: nameof(Message.MessageId),
               values: "Text is required");

            invalidMeshArgsException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshValidationException =
                 new MeshValidationException(
                     message: "Message validation errors occurred, please try again.",
                     innerException: invalidMeshArgsException);

            // when
            ValueTask<Message> getMessageTask =
                this.meshService.RetrieveMessageAsync(inputMessage.MessageId, invalidAuthorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    getMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(inputMessage.MessageId, invalidAuthorizationToken),
                    Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnGetMessageWithChunksIfResponseNullAsync()
        {
            // given
            string invalidAuthorizationToken = GetRandomString();
            int chunks = GetRandomNumber();
            Message randomMessage = CreateRandomMessage();
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = null;

            this.meshBrokerMock.Setup(broker =>
              broker.GetMessageAsync(inputMessage.MessageId, chunks.ToString(), invalidAuthorizationToken))
                  .ReturnsAsync(responseMessage);

            var nullHttpResponseMessageException =
                new NullHttpResponseMessageException(message: "HTTP Response Message is null.");

            var expectedMeshValidationException = new MeshValidationException(
                message: "Message validation errors occurred, please try again.",
                innerException: nullHttpResponseMessageException);

            // when
            ValueTask<Message> getMessageTask =
                this.meshService.RetrieveMessageAsync(inputMessage.MessageId, invalidAuthorizationToken, chunks);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    getMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(inputMessage.MessageId, chunks.ToString(), invalidAuthorizationToken),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
