// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Moq;
using NEL.MESH.Brokers.Chunks;
using NEL.MESH.Services.Foundations.Chunkings;
using Tynamix.ObjectFiller;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Chunks
{
    public partial class ChunkingServiceTests
    {
        private readonly Mock<IChunkingBroker> chunkingBrokerMock;
        private readonly IChunkingService chunkingService;

        public ChunkingServiceTests()
        {
            this.chunkingBrokerMock = new Mock<IChunkingBroker>();
            this.chunkingService = new ChunkingService(chunkingBroker: this.chunkingBrokerMock.Object);
        }

        private static string GetRandomString(int wordCount = 0)
        {
            return new MnemonicString(
                wordCount: wordCount == 0 ? GetRandomNumber() : wordCount,
                wordMinLength: 1,
                wordMaxLength: GetRandomNumber()).GetValue();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
