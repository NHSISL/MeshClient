// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using NEL.MESH.Models.Configurations;

namespace NEL.MESH.Brokers.Chunks
{
    internal class ChunkingBroker : IChunkingBroker
    {
        private readonly MeshConfiguration meshConfiguration;

        public ChunkingBroker(MeshConfiguration meshConfiguration)
        {
            this.meshConfiguration = meshConfiguration;
        }

        public float ChunkSizeInMegabytes => this.meshConfiguration.ChunkSizeInMegabytes;
    }
}
