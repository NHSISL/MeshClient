// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using YamlDotNet.Serialization;

namespace NEL.MESH.Infrastructure.Models.SetupDotNetTaskV4s
{
    internal class TargetDotNetVersionV4
    {
        [YamlMember(Alias = "dotnet-version")]
        public string? DotNetVersion { get; set; }
    }
}
