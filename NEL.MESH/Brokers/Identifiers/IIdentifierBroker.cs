// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace NEL.MESH.Brokers.Identifiers
{
    internal interface IIdentifierBroker
    {
        Guid GetIdentifier();
    }
}
