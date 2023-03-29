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
        public async Task ShouldSendFileAsync()
        {
            // given
            string randomToken = GetRandomString();
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;
            Message outputMessage = inputMessage.DeepClone();
            Message expectedMessage = outputMessage;

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            this.meshServiceMock.Setup(service =>
                service.SendFileAsync(inputMessage, randomToken))
                    .ReturnsAsync(outputMessage);

            // when
            Message actualMessage = await this.meshOrchestrationService
                .SendFileAsync(message: inputMessage);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.SendFileAsync(inputMessage, randomToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
