// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    internal class InvalidTokenException : Xeption
    {
        public InvalidTokenException()
            : base(message: "Token is invalid.") { }
    }
}
