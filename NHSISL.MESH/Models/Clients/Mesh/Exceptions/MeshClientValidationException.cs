// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using Xeptions;

namespace NHSISL.MESH.Models.Clients.Mesh.Exceptions
{
    public class MeshClientValidationException : Xeption
    {
        public MeshClientValidationException(Xeption innerException)
            : base(message: "Mesh client validation error(s) occurred, fix the error(s) and try again.",
                  innerException)
        { }

        public MeshClientValidationException(Xeption innerException, IDictionary data)
            : base(message: "Mesh client validation error(s) occurred, fix the error(s) and try again.",
                  innerException,
                  data)
        { }
    }
}
