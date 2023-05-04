// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Text;
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


        public static string GenerateRandomString(int chunkSizeInBytes, int chunksRequired)
        {
            var filler = new Filler<string>();
            var randomStrings = Enumerable.Range(0, chunksRequired)
                .Select(_ => filler.Create(chunkSizeInBytes));

            var bytes = Encoding.UTF8.GetBytes(string.Concat(randomStrings));
            return Encoding.UTF8.GetString(bytes);
        }


        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
