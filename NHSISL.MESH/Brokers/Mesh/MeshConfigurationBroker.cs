// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using NHSISL.MESH.Models.Configurations;

namespace NHSISL.MESH.Brokers.Mesh
{
    internal class MeshConfigurationBroker : IMeshConfigurationBroker
    {
        public int MaxChunkSizeInBytes { get; }
        public string MexFrom { get; }

        public MeshConfigurationBroker(MeshConfiguration MeshConfiguration)
        {
            MaxChunkSizeInBytes = MeshConfiguration.MaxChunkSizeInMegabytes * 1024 * 1024;
            MexFrom = MeshConfiguration.MailboxId;
        }
    }
}
