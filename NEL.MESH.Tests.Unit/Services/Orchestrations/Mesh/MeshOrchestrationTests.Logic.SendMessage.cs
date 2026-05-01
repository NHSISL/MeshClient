// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldSendFileAsync()
        {
            // given
            string randomToken = GetRandomString();
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;
            int randomChunkCount = GetRandomNumber();
            byte[] randomBytes = new byte[] { 1, 2, 3, 4 };
            using MemoryStream inputStream = new MemoryStream(randomBytes);

            List<(Message message, byte[] content)> chunkedInputMessages =
                new List<(Message, byte[])>();

            List<Message> chunkedOutputMessages = new List<Message>();
            string randomMessageId = GetRandomString();

            for (int i = 0; i < randomChunkCount; i++)
            {
                Message chunkMsg = CreateRandomSendMessage();
                chunkMsg.Headers["mex-chunk-range"] = new List<string> { $"{{{i + 1}:{randomChunkCount}}}" };
                byte[] chunkContent = new byte[] { (byte)i };
                chunkedInputMessages.Add((chunkMsg, chunkContent));

                Message outMsg = chunkMsg.DeepClone();
                outMsg.MessageId = randomMessageId;
                chunkedOutputMessages.Add(outMsg);
            }

            Message expectedMessage = chunkedOutputMessages[0].DeepClone();

            this.chunkServiceMock.Setup(service =>
                service.SplitStreamIntoChunks(inputMessage, inputStream))
                    .Returns(chunkedInputMessages);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(randomToken);

            for (int i = 0; i < chunkedInputMessages.Count; i++)
            {
                Message chunkMsg = chunkedInputMessages[i].message;
                byte[] chunkContent = chunkedInputMessages[i].content;

                if (i > 0)
                {
                    chunkMsg.MessageId = randomMessageId;
                }

                this.meshServiceMock.Setup(service =>
                    service.SendMessageAsync(chunkMsg, chunkContent, randomToken))
                        .ReturnsAsync(chunkedOutputMessages[i]);
            }

            // when
            Message actualMessage = await this.meshOrchestrationService
                .SendMessageAsync(message: inputMessage, content: inputStream);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.chunkServiceMock.Verify(service =>
                service.SplitStreamIntoChunks(inputMessage, inputStream),
                    Times.Once);

            this.tokenServiceMock.Verify(service =>
               service.GenerateTokenAsync(),
                   Times.Exactly(chunkedInputMessages.Count));

            for (int i = 0; i < chunkedInputMessages.Count; i++)
            {
                Message chunkMsg = chunkedInputMessages[i].message;
                byte[] chunkContent = chunkedInputMessages[i].content;

                this.meshServiceMock.Verify(service =>
                    service.SendMessageAsync(chunkMsg, chunkContent, randomToken),
                        Times.Once);
            }

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
