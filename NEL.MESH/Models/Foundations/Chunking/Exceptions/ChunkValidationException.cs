// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Chunking.Exceptions
{
    internal class ChunkValidationException : Xeption
    {
        public ChunkValidationException(Xeption innerException)
            : base(message: "Chunk validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
