// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace NEL.MESH.Models.Foundations.Mesh.ExternalModeld
{
    internal class HandshakeResponse
    {
        [JsonProperty(propertyName: "mailboxId")]
        public string MailboxId { get; set; }
    }
}
