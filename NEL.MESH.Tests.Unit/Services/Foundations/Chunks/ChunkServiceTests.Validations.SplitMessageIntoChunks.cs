// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Chunking.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Chunks
{
    public partial class ChunkServiceTests
    {
        [Fact]
        public void ShouldThrowValidationExceptionOnSplitMessageIntoChunksIfMessageIsNull()
        {
            // given
            Message nullMessage = null;
            using MemoryStream someStream = new MemoryStream(new byte[] { 1, 2, 3 });

            var nullMessageChunkException =
                new NullMessageChunkException(message: "Message chunk is null.");

            var expectedChunkValidationException =
                new ChunkValidationException(
                    message: "Chunk validation errors occurred, please try again.",
                    innerException: nullMessageChunkException as Xeption);

            // when
            Action splitMessageIntoChunksAction = () =>
                this.chunkService.SplitStreamIntoChunks(messageTemplate: nullMessage, content: someStream);

            ChunkValidationException actualChunkValidationException =
               Assert.Throws<ChunkValidationException>(splitMessageIntoChunksAction);

            // then
            actualChunkValidationException.Should()
                .BeEquivalentTo(expectedChunkValidationException);

            this.meshConfigurationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
