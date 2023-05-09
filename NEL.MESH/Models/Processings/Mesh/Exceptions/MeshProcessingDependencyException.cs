// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Mesh.Exceptions
{
    internal class MeshProcessingDependencyException : Xeption
    {
        public MeshProcessingDependencyException(Xeption innerException)
           : base(message: "Mesh processing dependency occurred, please try again.", innerException)
        { }
    }
}
