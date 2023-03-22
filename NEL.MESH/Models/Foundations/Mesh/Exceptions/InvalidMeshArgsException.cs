// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class InvalidMeshArgsException : Xeption
    {
        public InvalidMeshArgsException()
             : base(message: "Invalid Mesh argument(s), please correct the errors and try again.")
        { }
    }
}
