// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Mesh.Exceptions
{
    internal class MeshProcessingServiceException : Xeption
    {
        public MeshProcessingServiceException(Xeption innerException)
            : base(message: "Mesh processing service error occurred, contact support.",
                innerException)
        { }
    }
}
