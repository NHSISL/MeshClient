// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Microsoft.Extensions.Configuration;

namespace NHSISL.MESH.Tests.Integration.Witness
{
    public partial class ConfigurationTests
    {
        private readonly IConfiguration configuration;

        public ConfigurationTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("NEL_MESH_CLIENT_INTEGRATION_");

            this.configuration = configurationBuilder.Build();
        }
    }
}
