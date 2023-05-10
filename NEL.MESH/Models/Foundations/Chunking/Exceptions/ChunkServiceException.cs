// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Chunking.Exceptions
{
    internal class ChunkServiceException : Xeption
    {
        public ChunkServiceException(Xeption innerException)
            : base(message: "Chunk service error occurred, contact support.",
                innerException)
        { }
    }
}
