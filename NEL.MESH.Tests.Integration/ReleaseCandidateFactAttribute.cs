// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xunit;
using Xunit.Sdk;

namespace NEL.MESH.Tests.Integration
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        typeName: "NEL.MESH.Tests.Integration.ReleaseCandidateTestCaseDiscoverer",
        assemblyName: "NEL.MESH.Tests.Integration")]
    public class ReleaseCandidateFactAttribute : FactAttribute { }
}
