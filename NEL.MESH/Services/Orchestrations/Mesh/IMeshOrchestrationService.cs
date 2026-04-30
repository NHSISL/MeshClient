// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal interface IMeshOrchestrationService
    {
        ValueTask<bool> HandshakeAsync(CancellationToken cancellationToken = default);
        ValueTask<Message> SendMessageAsync(Message message, Stream content, CancellationToken cancellationToken = default);
        ValueTask<Message> TrackMessageAsync(string messageId, CancellationToken cancellationToken = default);
        ValueTask<List<string>> RetrieveMessagesAsync(CancellationToken cancellationToken = default);

        ValueTask<Message> RetrieveMessageAsync(
            string messageId,
            Stream content,
            CancellationToken cancellationToken = default);

        ValueTask<bool> AcknowledgeMessageAsync(string messageId, CancellationToken cancellationToken = default);
    }
}
