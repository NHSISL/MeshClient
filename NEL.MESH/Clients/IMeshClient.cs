﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using NEL.MESH.Clients.Mailboxes;

namespace NEL.MESH.Clients
{
    public interface IMeshClient
    {
        IMailboxClient Mailbox { get; }
    }
}
