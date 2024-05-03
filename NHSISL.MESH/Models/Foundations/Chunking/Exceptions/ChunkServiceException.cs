// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Foundations.Chunking.Exceptions
{
    internal class ChunkServiceException : Xeption
    {
        public ChunkServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
