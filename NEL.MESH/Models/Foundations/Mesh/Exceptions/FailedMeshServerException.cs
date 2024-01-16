// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class FailedMeshServerException : Xeption
    {
        public FailedMeshServerException(string message, Exception innerException)
            : base(message, innerException) { }

        public FailedMeshServerException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data) { }
    }
}
