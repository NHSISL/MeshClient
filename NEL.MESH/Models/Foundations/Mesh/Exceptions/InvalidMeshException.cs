// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class InvalidMeshException : Xeption
    {
        public InvalidMeshException()
            : base(message: "Invalid message, please correct errors and try again.") { }
    }
}
