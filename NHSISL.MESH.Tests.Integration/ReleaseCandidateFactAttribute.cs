// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xunit;
using Xunit.Sdk;

namespace NHSISL.MESH.Tests.Integration
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        typeName: "NHSISL.MESH.Tests.Integration.ReleaseCandidateTestCaseDiscoverer",
        assemblyName: "NHSISL.MESH.Tests.Integration")]
    public class ReleaseCandidateFactAttribute : FactAttribute { }
}
