// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Token.Exceptions
{
    public class InvalidMeshOrchestrationArgsException : Xeption
    {
        public InvalidMeshOrchestrationArgsException()
            : base(message: "Invalid mesh orchestraction argument valiation errors occurred, please correct the errors and try again.")
        { }
    }
}
