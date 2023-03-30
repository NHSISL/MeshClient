// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace NEL.MESH.Models.Foundations.Mesh.ExternalModels
{
    internal class SendFileResponse
    {
        [JsonProperty(propertyName: "messageID")]
        public string MessageId { get; set; }

        public string Message { get; set; }
    }
}
