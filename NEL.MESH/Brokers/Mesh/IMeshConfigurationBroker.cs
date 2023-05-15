// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.MESH.Brokers.Mesh
{
    internal interface IMeshConfigurationBroker
    {
        int MaxChunkSizeInBytes { get; }
        string MexFrom {  get; }
    }
}
