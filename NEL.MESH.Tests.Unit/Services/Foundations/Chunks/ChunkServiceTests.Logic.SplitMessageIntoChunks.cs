// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Chunks
{
    public partial class ChunkServiceTests
    {
        [Fact]
        public void ShouldSplitMessageIntoChunks()
        {
            // given
            int randomChunkSizeInBytes = GetRandomNumber();
            int randomChunkCount = GetRandomNumber();
            int additionalBytes = GetRandomNumber(min: 0, max: randomChunkSizeInBytes - 1);
            int randomBytesToGenerate = (randomChunkSizeInBytes * randomChunkCount) - additionalBytes;
            byte[] randomByteContent = GetRandomBytes(randomBytesToGenerate);

            List<byte[]> chunkParts =
                GetChunkedByteArrayContent(randomByteContent, chunkSizeInBytes: randomChunkSizeInBytes);

            int inputChunkSize = randomChunkSizeInBytes;
            int expectedByteCount = randomChunkSizeInBytes;
            int expectedChunkCount = randomChunkCount;

            Message randomMessage =
                CreateRandomSendMessage(byteArrayContent: randomByteContent);

            Message inputMessage = randomMessage;
            List<Message> outputMessages = new List<Message>();

            for (int i = 0; i < chunkParts.Count; i++)
            {
                Message chunk = new Message
                {
                    Headers = inputMessage.Headers,
                    FileContent = chunkParts[i]
                };

                SetMexChunkRange(chunk, item: i + 1, itemCount: chunkParts.Count);
                outputMessages.Add(chunk);
            }

            List<Message> expectedMessages = outputMessages.DeepClone();

            this.meshConfigurationBrokerMock.Setup(broker =>
                broker.MaxChunkSizeInBytes)
                    .Returns(value: inputChunkSize);

            // when
            List<Message> actualMessages = this.chunkService.SplitMessageIntoChunks(message: inputMessage);

            // then
            actualMessages.Count.Should().Be(expectedChunkCount);

            foreach (var message in actualMessages)
            {
                int actualByteCount = message.FileContent.Length;
                actualByteCount.Should().BeLessOrEqualTo(expectedByteCount);
            }

            byte[] combinedByteArrayContent = actualMessages.SelectMany(message => message.FileContent).ToArray();
            combinedByteArrayContent.Should().BeEquivalentTo(inputMessage.FileContent);

            this.meshConfigurationBrokerMock.Verify(broker =>
                broker.MaxChunkSizeInBytes,
                    Times.Once);

            this.meshConfigurationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
