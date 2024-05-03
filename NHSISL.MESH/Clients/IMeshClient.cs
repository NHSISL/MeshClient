// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------


// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using NHSISL.MESH.Clients.Mailboxes;

namespace NHSISL.MESH.Clients
{
    public interface IMeshClient
    {
        IMailboxClient Mailbox { get; }
    }
}
