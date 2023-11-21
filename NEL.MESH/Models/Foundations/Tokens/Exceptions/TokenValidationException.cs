// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Tokens.Exceptions
{
    internal class TokenValidationException : Xeption
    {
        public TokenValidationException(Xeption innerException, IDictionary data)
            : base(
                  message: "Token validation errors occurred, please try again.",
                  innerException,
                  data)
        { }
    }
}
