// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    internal class MeshOrchestrationDependencyValidationException : Xeption
    {
        public MeshOrchestrationDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
