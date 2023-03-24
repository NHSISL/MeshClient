// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Newtonsoft.Json;

namespace NEL.MESH.Models.Foundations.Mesh.ExternalModels
{
    internal class GetMessagesResponse
    {
        [JsonProperty(propertyName: "messages")]
        public List<string> Messages { get; set; }
    }
}