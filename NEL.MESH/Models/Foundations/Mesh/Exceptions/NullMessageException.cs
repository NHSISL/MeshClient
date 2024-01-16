// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class NullMessageException : Xeption
    {
        public NullMessageException(string message)
            : base(message) 
        { }
    }
}
