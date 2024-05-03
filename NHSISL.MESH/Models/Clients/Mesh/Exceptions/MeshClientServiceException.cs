// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Clients.Mesh.Exceptions
{
    public class MeshClientServiceException : Xeption
    {
        public MeshClientServiceException(Xeption innerException)
            : base(message: "Mesh client service error occurred, contact support.",
                  innerException)
        { }
    }
}
