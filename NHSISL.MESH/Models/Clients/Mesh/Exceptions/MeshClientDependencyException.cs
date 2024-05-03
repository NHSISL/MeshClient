// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Clients.Mesh.Exceptions
{
    public class MeshClientDependencyException : Xeption
    {
        public MeshClientDependencyException(Xeption innerException)
            : base(message: "Mesh client dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
