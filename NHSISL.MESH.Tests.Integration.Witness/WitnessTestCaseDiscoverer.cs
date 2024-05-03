// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NHSISL.MESH.Tests.Integration.Witness
{
    public class WitnessTestCaseDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink diagnosticMessageSink;
        private readonly IConfiguration configuration;

        public WitnessTestCaseDiscoverer(IMessageSink diagnosticMessageSink)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();
        }

        public IEnumerable<IXunitTestCase> Discover(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod,
            IAttributeInfo factAttribute)
        {
            var isReleaseCandidate = configuration.GetValue<bool>("RUN_WITNESS_TESTS");

            if (isReleaseCandidate)
            {
                yield return new XunitTestCase(
                    diagnosticMessageSink,
                    defaultMethodDisplay: discoveryOptions.MethodDisplayOrDefault(),
                    defaultMethodDisplayOptions: discoveryOptions.MethodDisplayOptionsOrDefault(),
                    testMethod);
            }
        }
    }
}
