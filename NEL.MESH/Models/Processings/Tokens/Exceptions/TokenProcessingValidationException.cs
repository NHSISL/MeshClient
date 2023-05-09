// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Tokens.Exceptions
{
    internal class TokenProcessingValidationException : Xeption
    {
        public TokenProcessingValidationException(Xeption innerException)
            : base(
                message: "Token processing validation errors occurred, please try again",
                innerException)
        { }
    }
}
