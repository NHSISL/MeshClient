// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class NullHeadersException : Xeption
    {
        public NullHeadersException(string message)
            : base(message) 
        { }
    }
}
