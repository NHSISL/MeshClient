// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace NEL.MESH.Models.Foundations.Mesh
{
    public class Message
    {
        public string MessageId { get; set; }
        public Dictionary<string, List<string>> Headers { get; set; } = new Dictionary<string, List<string>>();
        public byte[] FileContent { get; set; }
        public TrackingInfo TrackingInfo { get; set; }
    }
}
