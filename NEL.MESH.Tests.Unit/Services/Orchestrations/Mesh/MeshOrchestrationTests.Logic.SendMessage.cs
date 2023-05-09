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
        public async Task ShouldSendMessageAsync()
        {
            // given
            string randomToken = GetRandomString();
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;
            Message outputMessage = inputMessage.DeepClone();
            Message expectedMessage = outputMessage;

            this.tokenProcessingServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            this.meshProcessingServiceMock.Setup(service =>
                service.SendMessageAsync(inputMessage, randomToken))
                    .ReturnsAsync(outputMessage);

            // when
            Message actualMessage = await this.meshOrchestrationService
                .SendMessageAsync(message: inputMessage);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.tokenProcessingServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshProcessingServiceMock.Verify(service =>
                service.SendMessageAsync(inputMessage, randomToken),
                    Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.tokenProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
