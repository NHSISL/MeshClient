// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    internal class MeshOrchestrationValidationException : Xeption
    {
        public MeshOrchestrationValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
