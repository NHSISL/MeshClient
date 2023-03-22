// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using NEL.MESH.Clients.MeshClients;

namespace NEL.MESH.Clients
{
    public interface IMesh
    {
        IMeshClient MeshClient { get; }
    }
}
