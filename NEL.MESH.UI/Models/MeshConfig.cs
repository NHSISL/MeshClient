// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.MESH.UI.Models
{
    public class MeshConfig
    {
        public string MexClientVersion { get; set; } = string.Empty;
        public string MexOSName { get; set; } = string.Empty;
        public string MexOSVersion { get; set; } = string.Empty;
        public int ChunkSize { get; set; } = 20;
    }
}
