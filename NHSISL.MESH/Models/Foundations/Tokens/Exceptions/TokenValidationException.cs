// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Foundations.Tokens.Exceptions
{
    internal class TokenValidationException : Xeption
    {
        public TokenValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
