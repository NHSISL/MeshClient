// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Tokens.Exceptions
{
    internal class TokenDependencyException : Xeption
    {
        public TokenDependencyException(Xeption innerException)
            : base(message: "Token dependency error occurred, contact support.", innerException) { }
    }
}
