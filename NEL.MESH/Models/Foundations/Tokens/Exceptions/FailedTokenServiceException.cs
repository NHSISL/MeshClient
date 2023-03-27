// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Token.Exceptions
{
    internal class FailedTokenServiceException : Xeption
    {
        public FailedTokenServiceException(Exception innerException)
            : base(message: "Token service error occurred, contact support.", innerException) { }
    }
}
