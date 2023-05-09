// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;

namespace NEL.MESH.Services.Foundations.Chunks
{
    internal partial class ChunkService : IChunkService
    {
        private static void ValidateMessageIsNotNull(Message message)
        {
            if (message is null)
            {
                throw new NullMessageException();
            }
        }
    }
}
