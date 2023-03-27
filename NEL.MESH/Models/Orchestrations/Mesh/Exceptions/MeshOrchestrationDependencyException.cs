// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    internal class MeshOrchestrationDependencyException : Xeption
    {
        public MeshOrchestrationDependencyException(Xeption innerException)
            : base(
                message: "Mesh orchestration dependency error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
