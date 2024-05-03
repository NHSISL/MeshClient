// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xunit;
using Xunit.Sdk;

namespace NHSISL.MESH.Tests.Integration.Witness
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        typeName: "NHSISL.MESH.Tests.Integration.Witness.ReleaseCandidateTestCaseDiscoverer",
        assemblyName: "NHSISL.MESH.Tests.Integration.Witness")]
    public class ReleaseCandidateFactAttribute : FactAttribute { }
}
