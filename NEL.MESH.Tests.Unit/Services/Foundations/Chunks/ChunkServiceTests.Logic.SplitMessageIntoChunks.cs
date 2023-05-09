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
            string randomContent = GetRandomString(randomBytesToGenerate);


            int inputChunkSize = randomChunkSizeInBytes;
            int expectedByteCount = randomChunkSizeInBytes;
            int expectedChunkCount = randomChunkCount;
            Message randomMessage = CreateRandomSendMessage(stringContent: randomContent);
            Message inputMessage = randomMessage;

            List<Message> expectedMessages = new List<Message>();

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
