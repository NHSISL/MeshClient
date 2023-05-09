// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
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
            int randomChunkSize = GetRandomNumber();
            int expectedByteCount = randomChunkSize;
            int randomChunkCount = GetRandomNumber();
            int expectedChunkCount = randomChunkCount;
            string randomContent = GetRandomString(randomChunkSize, randomChunkCount);
            Message randomMessage = CreateRandomSendMessage(stringContent: randomContent);
            Message inputMessage = randomMessage;

            List<Message> expectedMessages = new List<Message>();

            this.meshConfigurationBrokerMock.Setup(broker =>
                broker.MaxChunkSizeInBytes)
                    .Returns(value: 20);

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
            this.meshConfigurationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
