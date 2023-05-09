// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Chunking.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Chunks
{
    public partial class ChunkServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfMessageIsNullAsync()
        {
            // given
            Message nullMessage = null;

            var nullMessageException =
                new NullMessageException();

            var expectedChunkValidationException =
                new ChunkValidationException(nullMessageException);

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
