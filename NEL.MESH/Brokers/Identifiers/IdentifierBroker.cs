// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace NEL.MESH.Brokers.Identifiers
{
    public class IdentifierBroker : IIdentifierBroker
    {
        public Guid GetIdentifier() =>
            Guid.NewGuid();
    }
}
