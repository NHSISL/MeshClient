// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xunit;
using Xunit.Sdk;

namespace NEL.MESH.Tests.Integration.Witness
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        typeName: "NEL.MESH.Tests.Integration.Witness.ReleaseCandidateTestCaseDiscoverer",
        assemblyName: "NEL.MESH.Tests.Integration.Witness")]
    public class ReleaseCandidateFactAttribute : FactAttribute { }
}
