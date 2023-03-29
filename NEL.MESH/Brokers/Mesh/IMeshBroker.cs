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

        ValueTask<HttpResponseMessage> HandshakeAsync();

        ValueTask<HttpResponseMessage> SendMessageAsync(
            string mailboxTo,
            string workflowId,
            string stringConent,
            string contentType,
            string localId,
            string subject,
            string fileName,
            string contentChecksum,
            string contentEncrypted,
            string encoding,
            string chunkRange,
            string authorizationToken);

        ValueTask<HttpResponseMessage> SendFileAsync(
            string mailboxTo,
            string workflowId,
            string contentType,
            byte[] fileContents,
            string fileName,
            string subject,
            string contentChecksum,
            string contentEncrypted,
            string encoding,
            string chunkRange,
            string localId,
            string authorizationToken);

        ValueTask<HttpResponseMessage> TrackMessageAsync(string messageId, string authorizationToken);
        ValueTask<HttpResponseMessage> GetMessagesAsync(string authorizationToken);
        ValueTask<HttpResponseMessage> GetMessageAsync(string messageId, string authorizationToken);
        ValueTask<HttpResponseMessage> AcknowledgeMessageAsync(string messageId, string authorizationToken);
    }
}
