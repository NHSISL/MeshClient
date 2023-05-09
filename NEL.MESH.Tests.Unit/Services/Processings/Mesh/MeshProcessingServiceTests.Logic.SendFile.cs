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
        public async Task ShouldSendFileAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message randomSendFile = CreateRandomMessage();
            randomSendFile.StringContent = null;
            Message inputMessage = randomSendFile;
            Message ouputMessage = inputMessage.DeepClone();
            inputMessage.MessageId = null;
            inputMessage.TrackingInfo = null;
            Message randomTrackingMessage = CreateRandomMessage();
            Message trackingOutputMessage = randomTrackingMessage;
            Message expectedMessage = ouputMessage.DeepClone();
            expectedMessage.TrackingInfo = trackingOutputMessage.TrackingInfo;

            this.meshServiceMock.Setup(service =>
                service.SendFileAsync(inputMessage, authorizationToken))
                    .ReturnsAsync(ouputMessage);

            this.meshServiceMock.Setup(service =>
                service.TrackMessageAsync(ouputMessage.MessageId, authorizationToken))
                    .ReturnsAsync(trackingOutputMessage);

            // when
            var actualMessage = await this.meshProcessingService
                .SendFileAsync(inputMessage, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshServiceMock.Verify(service =>
                service.SendFileAsync(inputMessage, authorizationToken),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.TrackMessageAsync(ouputMessage.MessageId, authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
