// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using NEL.MESH.Models.Foundations.Chunking.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xeptions;

namespace NEL.MESH.Services.Foundations.Chunks
{
    internal partial class ChunkService : IChunkService
    {
        private delegate List<Message> RetruningMessageListFunction();

        private List<Message> TryCatch(RetruningMessageListFunction retruningMessageListFunction)
        {
            try
            {
                return retruningMessageListFunction();
            }
            catch (NullMessageException nullMessageException)
            {
                throw CreateValidationException(nullMessageException);
            }
        }

        private ChunkValidationException CreateValidationException(Xeption exception)
        {
            var chunkValidationException = new ChunkValidationException(exception);

            return chunkValidationException;
        }
    }
}
