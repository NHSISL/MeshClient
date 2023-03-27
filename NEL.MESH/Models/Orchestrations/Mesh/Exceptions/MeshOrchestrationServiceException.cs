// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    internal class MeshOrchestrationServiceException : Xeption
    {
        public MeshOrchestrationServiceException(Exception innerException)
            : base(message: "Mesh orchestration service error occurred, contact support.", innerException) { }
    }
}
