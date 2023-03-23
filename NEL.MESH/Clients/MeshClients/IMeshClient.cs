// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Clients.MeshClients
{
    public interface IMeshClient
    {
        ValueTask<bool> HandshakeAsync();
        ValueTask<Message> SendMessageAsync(Message message);
        ValueTask<Message> SendFileAsync(Message message);
        ValueTask<Message> TrackMessageAsync(string messageId);
        ValueTask<List<string>> RetrieveMessagesAsync();
        ValueTask<Message> RetrieveMessageAsync(string messageId);
        ValueTask<bool> AcknowledgeMessageAsync(string messageId);
    }
}
