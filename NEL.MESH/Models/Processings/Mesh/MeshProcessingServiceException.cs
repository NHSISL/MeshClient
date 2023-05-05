// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    internal class MeshProcessingServiceException : Xeption
    {
        public MeshProcessingServiceException(Xeption innerException)
          : base(message: "Mesh processing service error occurred, contact support.",
                innerException)
        { }
    }
}
