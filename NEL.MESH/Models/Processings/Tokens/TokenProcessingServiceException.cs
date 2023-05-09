// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Token
{
    internal class TokenProcessingServiceException : Xeption
    {
        public TokenProcessingServiceException(Xeption innerException)
            : base(message: "Token processing service error occurred, contact support.",
                innerException)
        { }
    }
}
