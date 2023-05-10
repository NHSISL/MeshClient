// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using NEL.MESH.Models.Configurations;

namespace NEL.MESH.Brokers.Mesh
{
    internal class MeshConfigurationBroker : IMeshConfigurationBroker
    {
        public int MaxChunkSizeInBytes { get; }

        public MeshConfigurationBroker(MeshConfiguration MeshConfiguration)
        {
            MaxChunkSizeInBytes = MeshConfiguration.MaxChunkSizeInMegabytes * 1024 * 1024;
        }
    }
}
