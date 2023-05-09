// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Mesh.Exceptions
{
    internal class TokenProcessingDependencyException : Xeption
    {
        public TokenProcessingDependencyException(Xeption innerException)
           : base(message: "Token processing dependency occurred, please try again.", innerException)
        { }
    }
}
