// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Clients.Mesh.Exceptions
{
    internal class MeshClientServiceException : Xeption
    {
        public MeshClientServiceException(Xeption innerException)
            : base(message: "Mesh client service error occurred, contact support.",
                  innerException)
        { }
    }
}
