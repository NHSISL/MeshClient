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
            int randomChunkCount = GetRandomNumber();
            List<Message> randomChunkedMessages = CreateRandomChunkedSendMessages(randomChunkCount);
            List<Message> chunkedInputMessages = randomChunkedMessages;
            List<Message> chunkedOutputMessages = chunkedInputMessages.DeepClone();
            string randomMessageId = GetRandomString();
            chunkedOutputMessages[0].MessageId = randomMessageId;
            Message outputMessage = chunkedOutputMessages[0].DeepClone();
            outputMessage.StringContent = inputMessage.StringContent;
            Message expectedMessage = outputMessage.DeepClone();

            this.chunkServiceMock.Setup(service =>
                service.SplitMessageIntoChunks(inputMessage))
                    .Returns(chunkedInputMessages);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            for (int i = 0; i < chunkedInputMessages.Count; i++)
            {
                if (i == 0)
                {
                    this.meshServiceMock.Setup(service =>
                        service.SendMessageAsync(chunkedInputMessages[i], randomToken))
                            .ReturnsAsync(chunkedOutputMessages[0]);
                }
                else
                {
                    Message chunk = chunkedInputMessages[i];
                    chunk.MessageId = randomMessageId;

                    this.meshServiceMock.Setup(service =>
                        service.SendMessageAsync(chunk, randomToken))
                            .ReturnsAsync(chunkedOutputMessages[0]);
                }
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
                    Times.Exactly(chunkedInputMessages.Count));

            for (int i = 0; i < chunkedInputMessages.Count; i++)
            {
                if (i == 0)
                {
                    this.meshServiceMock.Verify(service =>
                        service.SendMessageAsync(chunkedInputMessages[i], randomToken),
                            Times.Once);
                }
                else
                {
                    Message chunk = chunkedInputMessages[i];
                    chunk.MessageId = randomMessageId;

                    this.meshServiceMock.Verify(service =>
                        service.SendMessageAsync(chunk, randomToken),
                            Times.Once);
                }
            }

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
