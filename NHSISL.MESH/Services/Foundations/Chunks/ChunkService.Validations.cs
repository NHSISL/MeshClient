// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using NHSISL.MESH.Models.Foundations.Chunking.Exceptions;
using NHSISL.MESH.Models.Foundations.Mesh;

namespace NHSISL.MESH.Services.Foundations.Chunks
{
    internal partial class ChunkService
    {
        private static void ValidateMessageIsNotNull(Message message)
        {
            if (message is null)
            {
                throw new NullMessageChunkException(message: "Message chunk is null.");
            }
        }
    }
}
