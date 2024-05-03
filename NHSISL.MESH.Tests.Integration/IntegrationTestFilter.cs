// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NEL.MESH.Tests.Integration
{
    public class AcceptanceTestDiscoverer : ITraitDiscoverer
    {
        private const string CategoryName = "AcceptanceTest";

        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>("Category", CategoryName);
        }

        public static bool ShouldRunAcceptanceTests()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            bool runAcceptanceTests = config.GetValue<bool>("runAcceptanceTests");
            return runAcceptanceTests;
        }

        public static bool IsAcceptanceTest(ITestCase testCase)
        {
            return testCase.Traits.Any(trait => trait.Key == "Category" && trait.Value.Contains(CategoryName));
        }

        public static bool SkipAcceptanceTests()
        {
            return !ShouldRunAcceptanceTests();
        }
    }

    public class AcceptanceTestFilter : ITestCaseDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public AcceptanceTestFilter(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        public IEnumerable<ITestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            var shouldSkip = AcceptanceTestDiscoverer.SkipAcceptanceTests();
            var isAcceptanceTest = AcceptanceTestDiscoverer.IsAcceptanceTest(new XunitTestCase(testMethod));

            if (shouldSkip && isAcceptanceTest)
            {
                _diagnosticMessageSink.OnMessage(new DiagnosticMessage($"Skipping {testMethod.Method.Name}"));
                yield break;
            }

            yield return new XunitTestCase(testMethod);
        }
    }
}
