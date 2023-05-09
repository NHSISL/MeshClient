// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Clients.Token.Exceptions
{
    public class TokenProcessingDependencyException : Xeption
    {
        public TokenProcessingDependencyException(Xeption innerException) :
            base(message: "Token processing dependency error occurred, contact support.", innerException)
        { }
    }
}
