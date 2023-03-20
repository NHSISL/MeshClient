// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class MeshDependencyValidationException : Xeption
    {
        public MeshDependencyValidationException(Xeption innerException)
            : base(message: "Mesh dependency error occurred, contact support.", innerException) { }
    }
}
