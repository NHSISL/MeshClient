// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NHSISL.MESH.Models.Foundations.Chunking.Exceptions
{
    public class FailedChunkServiceException : Xeption
    {
        public FailedChunkServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
