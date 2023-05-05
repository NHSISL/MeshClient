// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Processings.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfTokenIsNullOrEmptyAsync(string invalidText)
        {
            {
                // given
                string invalidAuthorizationToken = invalidText;
                Message randomSendMessage = CreateRandomMessage();
                randomSendMessage.FileContent = null;
                Message inputMessage = randomSendMessage;
                Message ouputMessage = inputMessage.DeepClone();
                inputMessage.MessageId = null;
                inputMessage.TrackingInfo = null;
                Message randomTrackingMessage = CreateRandomMessage();
                Message trackingOutputMessage = randomTrackingMessage;
                Message expectedMessage = ouputMessage.DeepClone();
                expectedMessage.TrackingInfo = trackingOutputMessage.TrackingInfo;

                var invalidArgumentsMeshProcessingException =
                    new InvalidArgumentsMeshProcessingException();

                invalidArgumentsMeshProcessingException.AddData(
                    key: "Token",
                    values: "Text is required");

                var expectedMeshProcessingValidationException =
                     new MeshProcessingValidationException(innerException: invalidArgumentsMeshProcessingException);

                // when
                ValueTask<Message> getMessagesTask =
                    this.meshProcessingService.SendMessageAsync(inputMessage, invalidAuthorizationToken);

                MeshProcessingValidationException actualMeshProcessingValidationException =
                    await Assert.ThrowsAsync<MeshProcessingValidationException>(() =>
                        getMessagesTask.AsTask());

                // then
                actualMeshProcessingValidationException.Should()
                    .BeEquivalentTo(expectedMeshProcessingValidationException);

                this.meshServiceMock.Verify(service =>
                    service.SendMessageAsync(inputMessage, invalidAuthorizationToken),
                        Times.Never);

                this.meshServiceMock.Verify(service =>
                    service.TrackMessageAsync(ouputMessage.MessageId, invalidAuthorizationToken),
                        Times.Never);

                this.meshServiceMock.VerifyNoOtherCalls();
            }
        }
    }
}