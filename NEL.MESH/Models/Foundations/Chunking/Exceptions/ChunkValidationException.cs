// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Chunking.Exceptions
{
    internal class ChunkValidationException : Xeption
    {
        public ChunkValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
