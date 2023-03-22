// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class MeshDependencyValidationException : Xeption
    {
        public MeshDependencyValidationException(Xeption innerException)
            : base(message: "Mesh dependency error occurred, contact support.", innerException) { }
    }
}
