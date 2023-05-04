// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using NEL.MESH.Brokers.Chunks;

namespace NEL.MESH.Services.Foundations.Chunkings
{
    internal partial class ChunkingService : IChunkingService
    {
        private readonly IChunkingBroker chunkingBroker;

        public ChunkingService(IChunkingBroker chunkingBroker)
        {
            this.chunkingBroker = chunkingBroker;
        }

        public byte[] CombineChunkedByteArrayContent(List<string> content) =>
            throw new System.NotImplementedException();

        public string CombineChunkedStringContent(List<string> content) =>
            throw new System.NotImplementedException();

        public List<byte[]> SplitByteContentIntoChunks(string content) =>
            throw new System.NotImplementedException();

        public List<string> SplitStringContentIntoChunks(string content) =>
            throw new System.NotImplementedException();
    }
}
