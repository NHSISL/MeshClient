// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Processings.Mesh
{
    public class FailedMeshProcessingServiceException : Xeption
    {
        public FailedMeshProcessingServiceException(Exception innerException)
            : base(message: "Failed mesh processing service error occurred, contact support.",
                innerException)
        { }
    }
}

