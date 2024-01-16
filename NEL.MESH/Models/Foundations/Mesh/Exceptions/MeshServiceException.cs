// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class MeshServiceException : Xeption
    {
        public MeshServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
