// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using NHSISL.MESH.Models.Foundations.Chunking.Exceptions;
using NHSISL.MESH.Models.Foundations.Mesh;
using Xeptions;
using Xunit;

namespace NHSISL.MESH.Tests.Unit.Services.Foundations.Chunks
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
                new ChunkValidationException(
                    message: "Chunk validation errors occurred, please try again.",
                    innerException: nullMessageChunkException as Xeption);

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
