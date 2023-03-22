// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class NullHeadersException : Xeption
    {
        public NullHeadersException()
            : base(message: "Message headers dictionary is null.") { }
    }
}
