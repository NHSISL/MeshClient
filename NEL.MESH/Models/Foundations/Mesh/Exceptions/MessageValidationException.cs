// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class MeshValidationException : Xeption
    {
        public MeshValidationException(Xeption innerException)
            : base(
                  message: "Message validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
