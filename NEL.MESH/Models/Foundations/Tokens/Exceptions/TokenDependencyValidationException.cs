// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Tokens.Exceptions
{
    internal class TokenDependencyValidationException : Xeption
    {
        public TokenDependencyValidationException(Xeption innerException, IDictionary data)
            : base(
                  message: "Token dependency error occurred, contact support.",
                  innerException,
                  data)
        { }
    }
}
