// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class FailedMeshServiceException : Xeption
    {
        public FailedMeshServiceException(Exception innerException)
            : base(message: "Mesh service error occurred, contact support.", innerException) { }
    }
}
