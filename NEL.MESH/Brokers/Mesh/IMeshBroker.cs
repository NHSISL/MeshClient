// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;

namespace NEL.MESH.Brokers.Mesh
{
    internal interface IMeshBroker
    {
        ValueTask<HttpResponseMessage> HandshakeAsync(string authorizationToken);

        ValueTask<HttpResponseMessage> SendMessageAsync(
            string mailboxTo,
            string workflowId,
            string localId,
            string subject,
            string fileName,
            string contentChecksum,
            string contentEncrypted,
            string encoding,
            string chunkRange,
            string contentType,
            string authorizationToken,
            string stringContent);

        ValueTask<HttpResponseMessage> SendFileAsync(
            string mailboxTo,
            string workflowId,
            string localId,
            string subject,
            string fileName,
            string contentChecksum,
            string contentEncrypted,
            string encoding,
            string chunkRange,
            string contentType,
            string authorizationToken,
            byte[] fileContents);

        ValueTask<HttpResponseMessage> TrackMessageAsync(string messageId, string authorizationToken);
        ValueTask<HttpResponseMessage> GetMessagesAsync(string authorizationToken);
        ValueTask<HttpResponseMessage> GetMessageAsync(string messageId, string authorizationToken);
        ValueTask<HttpResponseMessage> AcknowledgeMessageAsync(string messageId, string authorizationToken);
    }
}
