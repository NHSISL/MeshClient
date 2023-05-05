// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldSendMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message randomSendMessage = CreateRandomMessage();
            Message inputMessage = randomSendMessage;
            Message ouputMessage = inputMessage.DeepClone();
            inputMessage.MessageId = null;
            Message randomTrackingMessage = CreateRandomMessage();
            Message trackingOutputMessage = randomTrackingMessage;
            Message expectedMessage = ouputMessage.DeepClone();
            expectedMessage.TrackingInfo = trackingOutputMessage.TrackingInfo;

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(inputMessage, authorizationToken))
                    .ReturnsAsync(ouputMessage);

            this.meshServiceMock.Setup(service =>
                service.TrackMessageAsync(ouputMessage.MessageId, authorizationToken))
                    .ReturnsAsync(trackingOutputMessage);

            // when
            var actualMessage = await this.meshProcessingService
                .SendMessageAsync(inputMessage, authorizationToken);

            // then
            actualMessage.Should().Be(expectedMessage);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(inputMessage, authorizationToken),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.TrackMessageAsync(ouputMessage.MessageId, authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
