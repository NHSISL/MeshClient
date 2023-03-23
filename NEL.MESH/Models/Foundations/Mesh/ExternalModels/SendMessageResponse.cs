// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace NEL.MESH.Models.Foundations.Mesh.ExternalModeld
{
    internal class SendMessageResponse
    {
        [JsonProperty(propertyName: "messageID")]
        public string MessageId { get; set; }

        public string Message { get; set; }
    }
}
