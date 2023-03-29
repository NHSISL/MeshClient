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

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            this.meshServiceMock.Setup(service =>
                service.TrackMessageAsync(inputMessageId, randomToken))
                    .ReturnsAsync(outputMessage);

            // when
            Message actualMessage = await this.meshOrchestrationService
                .TrackMessageAsync(messageId: inputMessageId);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.TrackMessageAsync(inputMessageId, randomToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
