// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.MESH.Models.Foundations.Mesh
{
    public class TrackingInfo
    {
        public string AddressType { get; set; }
        public string Checksum { get; set; }
        public int ChunkCount { get; set; }
        public string CompressFlag { get; set; }
        public string DownloadTimestamp { get; set; }
        public string DtsId { get; set; }
        public string EncryptedFlag { get; set; }
        public string ExpiryTime { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string IsCompressed { get; set; }
        public string LocalId { get; set; }
        public string MeshRecipientOdsCode { get; set; }
        public string MessageId { get; set; }
        public string MessageType { get; set; }
        public string PartnerId { get; set; }
        public string Recipient { get; set; }
        public string RecipientName { get; set; }
        public string RecipientOrgCode { get; set; }
        public string RecipientSmtp { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string SenderOdsCode { get; set; }
        public string SenderOrgCode { get; set; }
        public string SenderSmtp { get; set; }
        public string Status { get; set; }
        public string StatusSuccess { get; set; }
        public string UploadTimestamp { get; set; }
        public string Version { get; set; }
        public string WorkflowId { get; set; }
    }
}
