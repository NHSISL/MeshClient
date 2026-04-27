// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class WitnessTestsTheoryAttribute : TheoryAttribute
    {
        public WitnessTestsTheoryAttribute()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            if (!configuration.GetValue<bool>("RUN_WITNESS_TESTS"))
            {
                Skip = "RUN_WITNESS_TESTS is not enabled";
            }
        }
    }
}
