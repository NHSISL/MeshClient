// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------


// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace NHSISL.MESH.Models.Foundations.Mesh.ExternalModels
{
    internal class SendMessageErrorResponse
    {
        [JsonProperty(propertyName: "messageID")]
        public string MessageId { get; set; }

        [JsonProperty(propertyName: "errorEvent")]
        public string ErrorEvent { get; set; }

        [JsonProperty(propertyName: "errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty(propertyName: "errorDescription")]
        public string ErrorDescription { get; set; }
    }
}