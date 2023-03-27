// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Token.Exceptions
{
    internal class TokenServiceException : Xeption
    {
        public TokenServiceException(Xeption innerException)
          : base(message: "Token service error occurred, contact support.",
                innerException)
        { }
    }
}
