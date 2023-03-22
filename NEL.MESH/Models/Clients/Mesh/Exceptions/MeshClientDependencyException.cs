// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Clients.Mesh.Exceptions
{
    internal class MeshClientDependencyException : Xeption
    {
        public MeshClientDependencyException(Xeption innerException)
            : base(message: "Mesh client dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
