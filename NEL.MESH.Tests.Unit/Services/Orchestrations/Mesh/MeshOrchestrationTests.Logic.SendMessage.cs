// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
            int randomChunkCount = GetRandomNumber();
            List<Message> randomChunkedMessages = CreateRandomChunkedSendMessages(randomChunkCount);
            List<Message> chunkedMessages = randomChunkedMessages;
            List<Message> chunkedSentMessages = chunkedMessages.DeepClone();

            this.chunkServiceMock.Setup(service =>
                service.SplitMessageIntoChunks(inputMessage))
                    .Returns(chunkedMessages);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            foreach (Message chunkedsentMessage in chunkedSentMessages)
            {
                this.meshServiceMock.Setup(service =>
                    service.SendMessageAsync(inputMessage, randomToken))
                        .ReturnsAsync(outputMessage);
            }

            // when
            Message actualMessage = await this.meshOrchestrationService
                .SendMessageAsync(message: inputMessage);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.chunkServiceMock.Verify(service =>
                service.SplitMessageIntoChunks(inputMessage),
                    Times.Once);


            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Exactly(chunkedMessages.Count));

            foreach (Message chunkedMessage in chunkedMessages)
            {
                this.meshServiceMock.Verify(service =>
                    service.SendMessageAsync(inputMessage, randomToken),
                        Times.Once);
            }

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
