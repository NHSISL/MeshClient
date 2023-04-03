// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace NEL.MESH.Models.Foundations.Mesh.ExternalModels
{
    internal class GetMessageResponse
    {
        [JsonProperty(propertyName: "message")]
        public string MessageId { get; set; }
    }
}