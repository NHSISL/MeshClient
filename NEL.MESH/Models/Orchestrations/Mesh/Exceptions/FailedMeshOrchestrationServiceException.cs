// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    public class FailedMeshOrchestrationServiceException : Xeption
    {
        public FailedMeshOrchestrationServiceException(Exception innerException)
            : base(message: "Failed mesh orchestration service occurred, please contact support", innerException) { }
    }
}
