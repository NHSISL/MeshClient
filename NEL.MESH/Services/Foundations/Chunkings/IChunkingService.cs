// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace NEL.MESH.Services.Foundations.Chunkings
{
    internal interface IChunkingService
    {
        List<string> SplitStringContentIntoChunks(string content);
        string CombineChunkedStringContent(List<string> content);
        List<byte[]> SplitByteContentIntoChunks(string content);
        byte[] CombineChunkedByteArrayContent(List<string> content);
    }
}
