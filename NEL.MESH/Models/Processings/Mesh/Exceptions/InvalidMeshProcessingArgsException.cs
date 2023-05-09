// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processing.Mesh.Exceptions
{
    public class InvalidMeshProcessingArgsException : Xeption
    {
        public InvalidMeshProcessingArgsException()
            : base(message: "Invalid mesh processing argument validation errors occurred, please correct the errors and try again.") { }
    }
}
