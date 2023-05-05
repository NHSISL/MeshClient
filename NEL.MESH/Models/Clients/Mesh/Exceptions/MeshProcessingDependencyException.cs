// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Clients.Mesh.Exceptions
{
    public class MeshProcessingDependencyException : Xeption
    {
        public MeshProcessingDependencyException(Xeption innerException) :
            base(message: "Mesh processing dependency error occurred, contact support.", innerException)
        { }
    }
}
