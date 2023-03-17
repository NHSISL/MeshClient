// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Security.Cryptography.X509Certificates;

namespace NEL.MESH.Models.Configurations
{
    internal interface IMeshApiConfiguration
    {
        string MailboxId { get; set; }
        string Password { get; set; }
        string Url { get; set; }
        X509Certificate2 RootCertificate { get; set; }
        X509Certificate2Collection IntermediateCertificates { get; set; }
        X509Certificate2 ClientCertificate { get; set; }
        string MexClientVersion { get; set; }
        string MexOSName { get; set; }
        string MexOSVersion { get; set; }
    }
}
