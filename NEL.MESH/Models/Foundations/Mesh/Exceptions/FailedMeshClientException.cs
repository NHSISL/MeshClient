// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class FailedMeshClientException : Xeption
    {
        public FailedMeshClientException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public FailedMeshClientException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data) { }
    }
}
