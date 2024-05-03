// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Foundations.Tokens.Exceptions
{
    internal class TokenDependencyValidationException : Xeption
    {
        public TokenDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
