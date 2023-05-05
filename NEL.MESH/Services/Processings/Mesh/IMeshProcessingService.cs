// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Processings.Mesh
{
    internal interface IMeshProcessingService
    {
        ValueTask<bool> HandshakeAsync(string authorizationToken);
        ValueTask<Message> SendMessageAsync(Message message, string authorizationToken);
        ValueTask<Message> SendFileAsync(Message message, string authorizationToken);
        ValueTask<Message> TrackMessageAsync(string messageId, string authorizationToken);
        ValueTask<List<string>> RetrieveMessagesAsync(string authorizationToken);
        ValueTask<Message> RetrieveMessageAsync(string messageId, string authorizationToken);
        ValueTask<bool> AcknowledgeMessageAsync(string messageId, string authorizationToken);
    }
}
