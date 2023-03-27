// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    internal class MeshOrchestrationDependencyValidationException : Xeption
    {
        public MeshOrchestrationDependencyValidationException(Xeption innerException)
            : base(
                message: "Mesh orchestration dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
