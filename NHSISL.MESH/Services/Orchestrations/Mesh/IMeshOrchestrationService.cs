// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NHSISL.MESH.Models.Foundations.Mesh;

namespace NHSISL.MESH.Services.Orchestrations.Mesh
{
    internal interface IMeshOrchestrationService
    {
        ValueTask<bool> HandshakeAsync();
        ValueTask<Message> SendMessageAsync(Message message);
        ValueTask<Message> TrackMessageAsync(string messageId);
        ValueTask<List<string>> RetrieveMessagesAsync();
        ValueTask<Message> RetrieveMessageAsync(string messageId);
        ValueTask<bool> AcknowledgeMessageAsync(string messageId);
    }
}
