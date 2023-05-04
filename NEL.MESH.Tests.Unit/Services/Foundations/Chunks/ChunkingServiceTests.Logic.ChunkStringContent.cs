// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Chunks
{
    public partial class ChunkingServiceTests
    {
        [Fact]
        public void ShouldSplitStringContentIntoChunks()
        {
            // given
            string randomContent = GetRandomString();
            string inputString = randomContent;
            int maxChunkSizeInBytes = 10;
            int expectedChunkCount = (int)Math.Ceiling((double)inputString.Length / maxChunkSizeInBytes);

            this.chunkingBrokerMock.Setup(broker =>
                broker.ChunkSizeInBytes)
                    .Returns(maxChunkSizeInBytes);

            // when
            List<string> actualChunks = this.chunkingService.SplitStringContentIntoChunks(randomContent);

            // then
            actualChunks.Should().HaveCount(expectedChunkCount);

            actualChunks.ForEach(chunk => (chunk.Length * sizeof(char))
                .Should().BeLessOrEqualTo(maxChunkSizeInBytes));

            string concatenatedString = actualChunks.Aggregate((current, next) => current + next);
            concatenatedString.Should().BeEquivalentTo(inputString);
            this.chunkingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
