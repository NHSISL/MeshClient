// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using Xunit.Abstractions;

namespace NEL.MESH.Tests.Integration
{
    public class IntegrationTestFilter : ITestCaseFilter
    {
        public bool IsMatch(ITestCase testCase)
        {
            var isIntegrationTest = testCase.TestMethod.TestClass.Class
                .GetCustomAttributes(typeof(TraitAttribute))
                .OfType<TraitAttribute>()
                .Any(t => t.Name == "Category" && t.Value == "Integration");

            if (isIntegrationTest)
            {
                var shouldRunIntegrationTests =
                    Environment.GetEnvironmentVariable("ShouldRunIntegrationTests");

                if (shouldRunIntegrationTests != null && shouldRunIntegrationTests.ToLower() == "false")
                {
                    return true; // skip the test
                }
            }

            return false; // run the test
        }
    }
}
