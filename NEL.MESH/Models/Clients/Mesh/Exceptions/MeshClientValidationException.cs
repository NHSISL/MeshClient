﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Models.Clients.Mesh.Exceptions
{
    internal class MeshClientValidationException : Xeption
    {
        public MeshClientValidationException(Xeption innerException)
            : base(message: "Mesh client validation error(s) occurred, fix the error(s) and try again.",
                  innerException)
        { }
    }
}