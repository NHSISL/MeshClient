// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Processings.Mesh.Exceptions
{
    public class NullMeshMessageProcessingException : Xeption
    {
        public NullMeshMessageProcessingException()
            : base(message: "Message is null.") { }
    }
}
