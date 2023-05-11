// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
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
            string randomContent = GetRandomString(bytesToGenerate: randomBytesToGenerate);
            List<string> chunkParts = GetChunks(content: randomContent, chunkSizeInBytes: randomChunkSizeInBytes);
            int expectedChunkCount = randomChunkCount;
            int inputChunkSize = randomChunkSizeInBytes;
            int expectedByteCount = randomChunkSizeInBytes;
            Message randomMessage = CreateRandomSendMessage(stringContent: randomContent);
            Message inputMessage = randomMessage;

            List<Message> outputMessages = new List<Message>();

            for (int i = 0; i < randomChunkCount; i++)
            {
                Message chunk = new Message
                {
                    Headers = inputMessage.Headers,
                    StringContent = chunkParts[i]
                };

                SetMexChunkRange(chunk, item: i + 1, itemCount: randomChunkCount);
                outputMessages.Add(chunk);
            }

            List<Message> expectedMessages = outputMessages;

            this.meshConfigurationBrokerMock.Setup(broker =>
                broker.MaxChunkSizeInBytes)
                    .Returns(value: inputChunkSize);

            // when
            List<Message> actualMessages = this.chunkService.SplitMessageIntoChunks(message: inputMessage);

            // then
            actualMessages.Count.Should().Be(expectedChunkCount);

            foreach (var message in actualMessages)
            {
                int actualByteCount = Encoding.UTF8.GetByteCount(message.StringContent);
                actualByteCount.Should().BeLessOrEqualTo(expectedByteCount);
            }

            string combinedStringContent = actualMessages
                .Aggregate("", (current, message) => current + message.StringContent);

            combinedStringContent.Should().BeEquivalentTo(inputMessage.StringContent);

            this.meshConfigurationBrokerMock.Verify(broker =>
                broker.MaxChunkSizeInBytes,
                    Times.Once);

            this.meshConfigurationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
