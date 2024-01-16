// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    public class FailedMeshOrchestrationServiceException : Xeption
    {
        public FailedMeshOrchestrationServiceException(string message, Exception innerException)
            : base(message, innerException) 
        { }
    }
}
