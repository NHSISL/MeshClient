// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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

        public List<string> SplitStringContentIntoChunks(string content)
        {
            List<string> chunks = new List<string>();
            int startIndex = 0;
            int maxChunkSizeInBytes = this.chunkingBroker.ChunkSizeInBytes;

            while (startIndex < content.Length)
            {
                int chunkSize = Math.Min(maxChunkSizeInBytes, content.Length - startIndex);

                string chunk = content.Substring(startIndex, chunkSize);
                chunks.Add(chunk);

                startIndex += chunkSize;
            }

            return chunks;
        }

        public string CombineChunkedStringContent(List<string> content) =>
            throw new System.NotImplementedException();

        public List<byte[]> SplitByteContentIntoChunks(string content) =>
            throw new System.NotImplementedException();

        public byte[] CombineChunkedByteArrayContent(List<string> content) =>
            throw new System.NotImplementedException();
    }
}
