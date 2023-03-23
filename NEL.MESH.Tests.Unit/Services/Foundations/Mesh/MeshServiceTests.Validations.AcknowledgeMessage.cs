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
        public async Task ShouldThrowValidationExceptionOnAcknowledgeMessageIfMessageIdIsNullOrEmptyAsync(string invalidText)
        {
            // given
            Message randomMessage = CreateRandomMessage();
            randomMessage.MessageId = invalidText;
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            this.meshBrokerMock.Setup(broker =>
                broker.AcknowledgeMessageAsync(inputMessage.MessageId))
                    .ReturnsAsync(responseMessage);

            var InvalidMeshArgsException =
                new InvalidMeshArgsException();

            InvalidMeshArgsException.AddData(
               key: nameof(Message.MessageId),
               values: "Text is required");

            var expectedMeshValidationException =
                 new MeshValidationException(
                    innerException: InvalidMeshArgsException,
                    validationSummary: GetValidationSummary(InvalidMeshArgsException.Data));

            // when
            ValueTask<bool> acknowledgeMessageTask =
                this.meshService.AcknowledgeMessageAsync(inputMessage.MessageId);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    acknowledgeMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.AcknowledgeMessageAsync(inputMessage.MessageId),
                    Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
