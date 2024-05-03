// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    public class InvalidMeshOrchestrationArgsException : Xeption
    {
        public InvalidMeshOrchestrationArgsException(string message)
            : base(message)
        { }
    }
}
