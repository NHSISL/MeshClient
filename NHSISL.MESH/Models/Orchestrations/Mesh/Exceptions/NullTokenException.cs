// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    public class InvalidTokenException : Xeption
    {
        public InvalidTokenException(string message)
            : base(message)
        { }
    }
}
