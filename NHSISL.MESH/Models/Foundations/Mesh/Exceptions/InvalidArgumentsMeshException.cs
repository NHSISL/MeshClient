// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NHSISL.MESH.Models.Foundations.Mesh.Exceptions
{
    public class InvalidArgumentsMeshException : Xeption
    {
        public InvalidArgumentsMeshException(string message)
            : base(message)
        { }
    }
}
