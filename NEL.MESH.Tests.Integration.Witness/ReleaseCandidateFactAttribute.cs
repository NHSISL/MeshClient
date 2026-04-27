// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ReleaseCandidateFactAttribute : FactAttribute
    {
        public ReleaseCandidateFactAttribute()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            if (!configuration.GetValue<bool>("IS_RELEASE_CANDIDATE"))
            {
                Skip = "IS_RELEASE_CANDIDATE is not enabled";
            }
        }
    }
}
