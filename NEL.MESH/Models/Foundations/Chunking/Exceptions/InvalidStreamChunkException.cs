// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Chunking.Exceptions
{
    internal class InvalidStreamChunkException : Xeption
    {
        public InvalidStreamChunkException(string message)
            : base(message)
        { }
    }
}
