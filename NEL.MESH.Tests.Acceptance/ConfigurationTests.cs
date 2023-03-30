// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Microsoft.Extensions.Configuration;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class ConfigurationTests
    {
        private readonly IConfiguration configuration;

        public ConfigurationTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("NEL:MESH:CLIENT:ACCEPTANCE:");

            this.configuration = configurationBuilder.Build();
        }


    }
}
