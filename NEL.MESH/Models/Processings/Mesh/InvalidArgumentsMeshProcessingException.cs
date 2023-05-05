// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Mesh
{
    public class InvalidArgumentsMeshProcessingException : Xeption
    {
        public InvalidArgumentsMeshProcessingException()
            : base(message:
                  "Invalid MESH argument processing valiation errors occurred, "
                  + "please correct the errors and try again.")
        { }
    }
}
