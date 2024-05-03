// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using NHSISL.MESH.Models.Foundations.Mesh;

namespace NHSISL.MESH.Services.Foundations.Chunks
{
    internal interface IChunkService
    {
        List<Message> SplitMessageIntoChunks(Message message);
    }
}
