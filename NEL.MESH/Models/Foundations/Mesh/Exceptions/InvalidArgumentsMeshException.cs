// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class InvalidArgumentsMeshException : Xeption
    {
        public InvalidArgumentsMeshException()
            : base(message:
                  "Invalid MESH argument valiation errors occurred, please correct the errors and try again.")
        { }
    }
}
