// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Mesh.Exceptions
{
    public class InvalidArgumentsMeshProcessingException : Xeption
    {
        public InvalidArgumentsMeshProcessingException()
            : base(message:
                "Invalid MESH argument processing validation errors occurred, "
                + "please correct the errors and try again.")
        { }
    }
}
