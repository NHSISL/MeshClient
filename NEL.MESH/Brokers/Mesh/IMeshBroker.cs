// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using NEL.MESH.Models.Configurations;

namespace NEL.MESH.Brokers.Mesh
{
    internal interface IMeshBroker
    {
        MeshConfiguration MeshConfiguration { get; }

        ValueTask<HttpResponseMessage> HandshakeAsync(string authorizationToken);

        ValueTask<HttpResponseMessage> SendMessageAsync(
            string authorizationToken,
            string mexFrom,
            string mexTo,
            string mexWorkflowId,
            string mexChunkRange,
            string mexSubject,
            string mexLocalId,
            string mexFileName,
            string mexContentChecksum,
            string contentType,
            string contentEncoding,
            string accept,
            byte[] fileContents);

        ValueTask<HttpResponseMessage> SendMessageAsync(
            string authorizationToken,
            string mexFrom,
            string mexTo,
            string mexWorkflowId,
            string mexChunkRange,
            string mexSubject,
            string mexLocalId,
            string mexFileName,
            string mexContentChecksum,
            string contentType,
            string contentEncoding,
            string accept,
            byte[] fileContents,
            string messageId,
            string chunkNumber);

        ValueTask<HttpResponseMessage> TrackMessageAsync(string messageId, string authorizationToken);
        ValueTask<HttpResponseMessage> GetMessagesAsync(string authorizationToken);
        ValueTask<HttpResponseMessage> GetMessageAsync(string messageId, string authorizationToken);
        ValueTask<HttpResponseMessage> GetMessageAsync(string messageId, string chunkNumber, string authorizationToken);
        ValueTask<HttpResponseMessage> AcknowledgeMessageAsync(string messageId, string authorizationToken);
    }
}
