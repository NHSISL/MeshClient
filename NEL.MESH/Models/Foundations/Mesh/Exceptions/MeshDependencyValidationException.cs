// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class MeshDependencyValidationException : Xeption
    {
        public MeshDependencyValidationException(Xeption innerException, IDictionary data)
            : base(
                  message: "Mesh dependency error occurred, contact support.",
                  innerException,
                  data)
        { }
    }
}
