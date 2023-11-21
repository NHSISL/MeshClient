// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using Xeptions;

namespace NEL.MESH.Models.Foundations.Mesh.Exceptions
{
    internal class MeshValidationException : Xeption
    {
        public MeshValidationException(Xeption innerException, IDictionary data)
            : base(
                  message: "Message validation errors occurred, please try again.",
                  innerException,
                  data)
        { }
    }
}
