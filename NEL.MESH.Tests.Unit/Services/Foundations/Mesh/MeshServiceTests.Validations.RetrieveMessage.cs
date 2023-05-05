// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessage(
                inputMessage,
                new Dictionary<string, List<string>>(),
                new Dictionary<string, List<string>>());

            this.meshBrokerMock.Setup(broker =>
              broker.GetMessageAsync(inputMessage.MessageId, invalidAuthorizationToken))
                  .ReturnsAsync(responseMessage);

            var InvalidMeshArgsException =
                new InvalidArgumentsMeshException();

            InvalidMeshArgsException.AddData(
               key: nameof(Message.MessageId),
               values: "Text is required");

            InvalidMeshArgsException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshValidationException =
                 new MeshValidationException(innerException: InvalidMeshArgsException);

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
    }
}
