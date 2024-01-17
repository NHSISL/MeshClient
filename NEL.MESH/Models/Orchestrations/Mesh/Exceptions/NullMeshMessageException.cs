// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Orchestrations.Mesh.Exceptions
{
    public class NullMeshMessageException : Xeption
    {
        public NullMeshMessageException(string message)
            : base(message)
        { }
    }
}
