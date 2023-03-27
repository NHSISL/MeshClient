// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Security.Cryptography.X509Certificates;

namespace NEL.MESH.Models.Configurations
{
    public class MeshConfiguration
    {
        public string MailboxId { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
        public X509Certificate2 RootCertificate { get; set; }
        public X509Certificate2Collection IntermediateCertificates { get; set; }
        public X509Certificate2 ClientCertificate { get; set; }
        public string MexClientVersion { get; set; }
        public string MexOSName { get; set; }
        public string MexOSVersion { get; set; }
    }
}
