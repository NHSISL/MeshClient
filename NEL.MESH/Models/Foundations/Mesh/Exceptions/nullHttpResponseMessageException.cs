// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class NullHttpResponseMessageException : Xeption
    {
        public NullHttpResponseMessageException()
            : base(message: "HTTP Response Message is null.") { }
    }
}
