// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    public class InvalidMeshOrchestrationArgsException : Xeption
    {
        public InvalidMeshOrchestrationArgsException()
            : base(message: "Invalid mesh orchestration argument validation errors occurred, please correct the errors and try again.") { }
    }
}
