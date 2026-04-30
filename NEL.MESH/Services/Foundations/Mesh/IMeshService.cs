// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Foundations.Mesh
{
    internal interface IMeshService
    {
        ValueTask<bool> HandshakeAsync(string authorizationToken, CancellationToken cancellationToken = default);

        ValueTask<Message> SendMessageAsync(
            Message message,
            byte[] fileContent,
            string authorizationToken,
            CancellationToken cancellationToken = default);

        ValueTask<Message> TrackMessageAsync(
            string messageId,
            string authorizationToken,
            CancellationToken cancellationToken = default);

        ValueTask<List<string>> RetrieveMessagesAsync(
            string authorizationToken,
            CancellationToken cancellationToken = default);

        ValueTask<Message> RetrieveMessageAsync(
            string messageId,
            string authorizationToken,
            int chunkPart = 1,
            CancellationToken cancellationToken = default);

        ValueTask<Message> RetrieveMessageAsync(
            string messageId,
            string authorizationToken,
            Stream outputStream,
            int chunkPart = 1,
            CancellationToken cancellationToken = default);

        ValueTask<bool> AcknowledgeMessageAsync(
            string messageId,
            string authorizationToken,
            CancellationToken cancellationToken = default);
    }
}
