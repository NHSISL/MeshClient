// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            byte[] randomByteContent = GetRandomBytes(randomBytesToGenerate);

            List<byte[]> chunkParts =
                GetChunkedByteArrayContent(randomByteContent, chunkSizeInBytes: randomChunkSizeInBytes);

            int inputChunkSize = randomChunkSizeInBytes;
            int expectedByteCount = randomChunkSizeInBytes;
            int expectedChunkCount = randomChunkCount;

            Message randomMessage = CreateRandomSendMessage(byteArrayContent: randomByteContent);
            Message inputMessage = randomMessage;
            using MemoryStream inputStream = new MemoryStream(randomByteContent);

            this.meshConfigurationBrokerMock.Setup(broker =>
                broker.MaxChunkSizeInBytes)
                    .Returns(value: inputChunkSize);

            // when
            List<(Message message, byte[] content)> actualChunks =
                this.chunkService.SplitStreamIntoChunks(inputMessage, inputStream).ToList();

            // then
            actualChunks.Count.Should().Be(expectedChunkCount);

            foreach (var (_, content) in actualChunks)
            {
                content.Length.Should().BeLessOrEqualTo(expectedByteCount);
            }

            byte[] combinedByteArrayContent = actualChunks.SelectMany(c => c.content).ToArray();
            combinedByteArrayContent.Should().BeEquivalentTo(randomByteContent);

            this.meshConfigurationBrokerMock.Verify(broker =>
                broker.MaxChunkSizeInBytes,
                    Times.Once);

            this.meshConfigurationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
