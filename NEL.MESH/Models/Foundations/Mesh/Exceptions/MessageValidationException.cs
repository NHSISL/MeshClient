// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class MeshValidationException : Xeption
    {
        public MeshValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}
