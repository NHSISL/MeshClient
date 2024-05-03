// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Foundations.Chunking.Exceptions
{
    internal class NullMessageChunkException : Xeption
    {
        public NullMessageChunkException(string message)
            : base(message)
        { }
    }
}
