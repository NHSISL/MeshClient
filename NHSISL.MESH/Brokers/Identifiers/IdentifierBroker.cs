// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace NHSISL.MESH.Brokers.Identifiers
{
    internal class IdentifierBroker : IIdentifierBroker
    {
        public Guid GetIdentifier() =>
            Guid.NewGuid();
    }
}
