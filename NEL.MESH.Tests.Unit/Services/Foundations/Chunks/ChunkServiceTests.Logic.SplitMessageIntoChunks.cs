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

        [Fact]
        public void ShouldCalculateChunkCountCorrectlyForLargeStreams()
        {
            // given
            const int maxPartSize = 100 * 1024 * 1024; // 100 MB chunks
            const long largeFileSize = 3L * 1024 * 1024 * 1024; // 3 GB: overflows int if cast naively
            const int expectedChunkCount = 31; // ceil(3 * 1024 / 100)

            Message randomMessage = CreateRandomSendMessage(byteArrayContent: new byte[0]);
            Message inputMessage = randomMessage;

            using LargeVirtualStream inputStream = new LargeVirtualStream(largeFileSize);

            this.meshConfigurationBrokerMock.Setup(broker =>
                broker.MaxChunkSizeInBytes)
                    .Returns(maxPartSize);

            // when
            int actualChunkCount =
                this.chunkService.SplitStreamIntoChunks(inputMessage, inputStream).Count();

            // then
            actualChunkCount.Should().Be(expectedChunkCount);

            this.meshConfigurationBrokerMock.Verify(broker =>
                broker.MaxChunkSizeInBytes,
                    Times.Once);

            this.meshConfigurationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
