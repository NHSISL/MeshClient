// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace NHSISL.MESH.Models.Foundations.Tokens.Exceptions
{
    public class FailedTokenServiceException : Xeption
    {
        public FailedTokenServiceException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
