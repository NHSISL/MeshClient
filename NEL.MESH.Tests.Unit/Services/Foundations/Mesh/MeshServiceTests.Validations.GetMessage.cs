// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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

            Message randomMessage = CreateRandomMessage();
            randomMessage.MessageId = invalidText;
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            this.meshBrokerMock.Setup(broker =>
              broker.GetMessageAsync(inputMessage.MessageId))
                  .ReturnsAsync(responseMessage);

            var InvalidMeshArgsException =
                new InvalidMeshArgsException();

            InvalidMeshArgsException.AddData(
               key: nameof(Message.MessageId),
               values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(InvalidMeshArgsException);

            // when
            ValueTask<Message> getMessageTask =
                this.meshService.GetMessageAsync(inputMessage.MessageId);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    getMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(inputMessage.MessageId),
                    Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
