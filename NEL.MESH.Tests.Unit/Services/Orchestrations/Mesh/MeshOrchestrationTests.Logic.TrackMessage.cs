// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Fact]
        public async Task ShouldTrackMessageAsync()
        {
            // given
            string randomToken = GetRandomString();
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            Message randomMessage = CreateRandomSendMessage();
            Message outputMessage = randomMessage.DeepClone();
            Message expectedMessage = outputMessage;

            this.tokenProcessingServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            this.meshProcessingServiceMock.Setup(service =>
                service.TrackMessageAsync(inputMessageId, randomToken))
                    .ReturnsAsync(outputMessage);

            // when
            Message actualMessage = await this.meshOrchestrationService
                .TrackMessageAsync(messageId: inputMessageId);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.tokenProcessingServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshProcessingServiceMock.Verify(service =>
                service.TrackMessageAsync(inputMessageId, randomToken),
                    Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.tokenProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
