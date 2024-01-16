// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class FailedMeshServiceException : Xeption
    {
        public FailedMeshServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
