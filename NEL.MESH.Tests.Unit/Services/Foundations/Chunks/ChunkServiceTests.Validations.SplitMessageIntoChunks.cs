// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Chunking.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Chunks
{
    public partial class ChunkServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSplitMessageIntoChunksIfMessageIsNullAsync()
        {
            // given
            Message nullMessage = null;

            var nullMessageChunkException =
                new NullMessageChunkException(message: "Message chunk is null.");

            var expectedChunkValidationException =
                new ChunkValidationException(nullMessageChunkException);

            // when
            Action splitMessageIntoChunksAction = () =>
                this.chunkService.SplitMessageIntoChunks(message: nullMessage);

            ChunkValidationException actualChunkValidationException =
               Assert.Throws<ChunkValidationException>(splitMessageIntoChunksAction);

            // then
            actualChunkValidationException.Should()
                .BeEquivalentTo(expectedChunkValidationException);

            this.meshConfigurationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
