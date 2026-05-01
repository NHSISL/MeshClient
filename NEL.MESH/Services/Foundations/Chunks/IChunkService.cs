// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Foundations.Chunks
{
    internal interface IChunkService
    {
        IEnumerable<(Message message, byte[] content)> SplitStreamIntoChunks(Message messageTemplate, Stream content);
    }
}
