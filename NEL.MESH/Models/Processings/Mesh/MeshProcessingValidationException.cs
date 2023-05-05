// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Mesh
{
    internal class MeshProcessingValidationException : Xeption
    {
        public MeshProcessingValidationException(Xeption innerException)
            : base(
                message: "Mesh processing validation errors occurred, please try again",
                innerException)
        { }
    }
}
