// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Foundations.Tokens.Exceptions
{
    public class InvalidTokenArgsException : Xeption
    {
        public InvalidTokenArgsException(string message)
            : base(message)
        { }
    }
}
