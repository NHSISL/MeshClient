// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
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
        public async Task ShouldRetrieveMessageAsync()
        {
            // given
            string randomToken = GetRandomString();
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            Message randomMessage = CreateRandomSendMessage();
            Message outputMessage = randomMessage.DeepClone();
            Message expectedMessage = outputMessage;
            using MemoryStream outputStream = new MemoryStream();

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageAsync(
                    inputMessageId, randomToken, It.IsAny<Stream>(), 1))
                        .ReturnsAsync(outputMessage);

            // when
            Message actualMessage = await this.meshOrchestrationService
                .RetrieveMessageAsync(messageId: inputMessageId, content: outputStream);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(
                    inputMessageId, randomToken, It.IsAny<Stream>(), 1),
                        Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
