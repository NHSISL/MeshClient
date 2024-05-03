// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------


// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace NHSISL.MESH.Models.Foundations.Mesh.ExternalModels
{
    internal class HandshakeResponse
    {
        [JsonProperty(propertyName: "mailboxId")]
        public string MailboxId { get; set; }
    }
}
