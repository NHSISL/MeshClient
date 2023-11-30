﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class FailedMeshServerException : Xeption
    {
        public FailedMeshServerException(Exception innerException)
            : base(message: "Mesh server error occurred, contact support.", innerException) { }

        public FailedMeshServerException(Exception innerException, IDictionary data)
            : base(message: "Mesh server error occurred, contact support.", innerException, data) { }
    }
}
