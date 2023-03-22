// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class FailedMeshServerException : Xeption
    {
        public FailedMeshServerException(Exception innerException)
            : base(message: "Mesh server error occurred, contact support.", innerException) { }
    }
}
