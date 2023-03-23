// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh.ExternalModeld;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnTrackMessageIfRequiredMessageArgumenstAreinvalidAsync(
            string invalidInput)
        {
            // given
            dynamic randomTrackingProperties = CreateRandomTrackingProperties();
            string randomString = GetRandomString();
            string inputMessageId = invalidInput;
            Message randomMessage = CreateRandomMessage();
            randomMessage.MessageId = inputMessageId;

            randomMessage.TrackingInfo =
                MapDynamicObjectToTrackingInfo(randomTrackingProperties);

            TrackMessageResponse trackMessageResponse =
                MapDynamicObjectToTrackMessageResponse(randomTrackingProperties);

            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateTrackingHttpResponseMessage(trackMessageResponse);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(inputMessageId))
                    .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageFromTrackingHttpResponseMessage(inputMessageId, responseMessage);

            var invalidMeshArgsException =
                new InvalidMeshArgsException();

            invalidMeshArgsException.AddData(
                key: nameof(Message.MessageId),
                values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                   innerException: invalidMeshArgsException,
                   validationSummary: GetValidationSummary(invalidMeshArgsException.Data));

            // when
            ValueTask<Message> trackMessageTask =
                this.meshService.TrackMessageAsync(inputMessageId);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    trackMessageTask.AsTask());

            // then
            actualMeshValidationException.Should().BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(inputMessageId),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
