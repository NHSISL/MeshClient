// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Tokens
{
    internal class TokenProcessingDependencyValidationException : Xeption
    {
        public TokenProcessingDependencyValidationException(Xeption innerException)
           : base(message: "Token processing dependency validation occurred, please try again.", innerException)
        { }
    }
}
