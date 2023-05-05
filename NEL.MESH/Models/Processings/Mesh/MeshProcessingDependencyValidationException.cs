// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Mesh
{
    public class MeshProcessingDependencyValidationException : Xeption
    {
        public MeshProcessingDependencyValidationException(Xeption innerException)
           : base(message: "Mesh processing dependency validation occurred, please try again.", innerException)
        { }
    }
}
