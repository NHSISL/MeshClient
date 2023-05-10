// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Chunking.Exceptions
{
    public class FailedChunkServiceException : Xeption
    {
        public FailedChunkServiceException(Exception innerException)
            : base(message: "Chunk service error occurred, contact support.", 
                innerException) 
        { }
    }
}
