// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using YamlDotNet.Serialization;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;

namespace NEL.MESH.Infrastructure.Models.SetupDotNetTaskV4s
{
    internal class SetupDotNetTaskV4 : GithubTask
    {
        [YamlMember(Order = 4, DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public new string Uses { get; private set; } = "actions/setup-dotnet@v4";

        [YamlMember(Order = 5, DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public new TargetDotNetVersionV4? With { get; set; }
    }
}
