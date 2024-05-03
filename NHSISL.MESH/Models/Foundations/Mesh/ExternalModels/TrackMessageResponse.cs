// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------


// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace NHSISL.MESH.Models.Foundations.Mesh.ExternalModels
{
    internal class TrackMessageResponse
    {
        [JsonProperty(propertyName: "addressType")]
        public string AddressType { get; set; }

        [JsonProperty(propertyName: "checksum")]
        public string Checksum { get; set; }

        [JsonProperty(propertyName: "chunkCount")]
        public int ChunkCount { get; set; }

        [JsonProperty(propertyName: "compressFlag")]
        public string CompressFlag { get; set; }

        [JsonProperty(propertyName: "downloadTimestamp")]
        public string DownloadTimestamp { get; set; }

        [JsonProperty(propertyName: "dtsId")]
        public string DtsId { get; set; }

        [JsonProperty(propertyName: "encryptedFlag")]
        public string EncryptedFlag { get; set; }

        [JsonProperty(propertyName: "expiryTime")]
        public string ExpiryTime { get; set; }

        [JsonProperty(propertyName: "fileName")]
        public string FileName { get; set; }

        [JsonProperty(propertyName: "fileSize")]
        public int FileSize { get; set; }

        [JsonProperty(propertyName: "isCompressed")]
        public string IsCompressed { get; set; }

        [JsonProperty(propertyName: "localId")]
        public string LocalId { get; set; }

        [JsonProperty(propertyName: "meshRecipientOdsCode")]
        public string MeshRecipientOdsCode { get; set; }

        [JsonProperty(propertyName: "messageId")]
        public string MessageId { get; set; }

        [JsonProperty(propertyName: "messageType")]
        public string MessageType { get; set; }

        [JsonProperty(propertyName: "partnerId")]
        public string PartnerId { get; set; }

        [JsonProperty(propertyName: "recipient")]
        public string Recipient { get; set; }

        [JsonProperty(propertyName: "recipientName")]
        public string RecipientName { get; set; }

        [JsonProperty(propertyName: "recipientOrgCode")]
        public string RecipientOrgCode { get; set; }

        [JsonProperty(propertyName: "recipientSmtp")]
        public string RecipientSmtp { get; set; }

        [JsonProperty(propertyName: "sender")]
        public string Sender { get; set; }

        [JsonProperty(propertyName: "senderName")]
        public string SenderName { get; set; }

        [JsonProperty(propertyName: "senderOdsCode")]
        public string SenderOdsCode { get; set; }

        [JsonProperty(propertyName: "senderOrgCode")]
        public string SenderOrgCode { get; set; }

        [JsonProperty(propertyName: "senderSmtp")]
        public string SenderSmtp { get; set; }

        [JsonProperty(propertyName: "status")]
        public string Status { get; set; }

        [JsonProperty(propertyName: "statusSuccess")]
        public string StatusSuccess { get; set; }

        [JsonProperty(propertyName: "uploadTimestamp")]
        public string UploadTimestamp { get; set; }

        [JsonProperty(propertyName: "version")]
        public string Version { get; set; }

        [JsonProperty(propertyName: "workflowId")]
        public string WorkflowId { get; set; }

    }
}
