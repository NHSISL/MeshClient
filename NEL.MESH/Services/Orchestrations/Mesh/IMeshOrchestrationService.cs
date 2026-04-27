// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal interface IMeshOrchestrationService
    {
        ValueTask<bool> HandshakeAsync();
        ValueTask<Message> SendMessageAsync(Message message);
        ValueTask<Message> TrackMessageAsync(string messageId);
        ValueTask<List<string>> RetrieveMessagesAsync();

        [Obsolete("This method is obsolete. Use RetrieveMessageAsync(string messageId, Stream outputStream) " +
            "instead to avoid memory issues with large files.")]
        ValueTask<Message> RetrieveMessageAsync(string messageId);

        ValueTask<Message> RetrieveMessageAsync(string messageId, Stream outputStream);
        ValueTask<bool> AcknowledgeMessageAsync(string messageId);
    }
}
