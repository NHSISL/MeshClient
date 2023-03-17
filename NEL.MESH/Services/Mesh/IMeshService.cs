// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Mesh
{
    internal interface IMeshService
    {
        ValueTask<bool> HandshakeAsync();
        ValueTask<Message> SendMessageAsync(Message message);
        ValueTask<Message> SendFileAsync(Message message);
        ValueTask<HttpResponseMessage> TrackMessageAsync(string messageId);
        ValueTask<List<string>> GetMessagesAsync();
        ValueTask<Message> GetMessageAsync(string messageId);
        ValueTask<HttpResponseMessage> AcknowledgeMessageAsync(string messageId);
    }
}
