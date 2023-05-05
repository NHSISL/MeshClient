// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    internal class MeshOrchestrationValidationException : Xeption
    {
        public MeshOrchestrationValidationException(Xeption innerException)
            : base(
                  message: "Mesh orchestration validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
