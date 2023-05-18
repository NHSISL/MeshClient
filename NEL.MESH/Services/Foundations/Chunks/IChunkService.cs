// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Foundations.Chunks
{
    internal interface IChunkService
    {
        List<Message> SplitMessageIntoChunks(Message message);
    }
}
