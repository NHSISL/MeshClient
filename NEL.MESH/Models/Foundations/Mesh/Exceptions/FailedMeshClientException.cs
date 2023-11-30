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
        public FailedMeshClientException(Exception innerException)
            : base(message: "Mesh client error occurred, contact support.", innerException) { }

        public FailedMeshClientException(Exception innerException, IDictionary data)
            : base(message: "Mesh client error occurred, contact support.", innerException, data) { }
    }
}
