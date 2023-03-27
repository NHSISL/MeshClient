// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Token.Exceptions
{
    internal class InvalidTokenArgsException : Xeption
    {
        public InvalidTokenArgsException()
            : base(message: "Invalid token argument(s), please correct the errors and try again.")
        { }
    }
}
