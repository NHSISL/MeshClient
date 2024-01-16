// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Tokens.Exceptions
{
    internal class TokenDependencyException : Xeption
    {
        public TokenDependencyException(string message, Xeption innerException)
            : base(message, innerException) 
        { }
    }
}
