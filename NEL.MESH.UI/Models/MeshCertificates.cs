// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.MESH.UI.Models
{
    public class MeshCertificates
    {
        public string Environment { get; set; } = string.Empty;
        public string ClientCertificate { get; set; } = string.Empty;
        public string RootCertificate { get; set; } = string.Empty;
        public List<string> IntermediateCertificates { get; set; } = new List<string>();
    }
}
