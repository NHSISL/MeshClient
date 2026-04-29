// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal interface IMeshOrchestrationService
    {
        ValueTask<bool> HandshakeAsync();
        ValueTask<Message> SendMessageAsync(Message message, Stream content);
        ValueTask<Message> TrackMessageAsync(string messageId);
        ValueTask<List<string>> RetrieveMessagesAsync();
        ValueTask<Message> RetrieveMessageAsync(string messageId, Stream content);
        ValueTask<bool> AcknowledgeMessageAsync(string messageId);
    }
}
