// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class FailedMeshClientException : Xeption
    {
        public FailedMeshClientException(Exception innerException)
            : base(message: "Mesh client error occurred, contact support.", innerException: innerException) { }
    }
}
