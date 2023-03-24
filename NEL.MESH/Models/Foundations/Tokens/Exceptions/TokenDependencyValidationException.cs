// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Token.Exceptions
{
    internal class TokenDependencyValidationException : Xeption
    {
        public TokenDependencyValidationException(Xeption innerException)
            : base(message: "Token dependency error occurred, contact support.", innerException) { }
    }
}
