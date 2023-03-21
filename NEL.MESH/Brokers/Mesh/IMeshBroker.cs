// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;

namespace NEL.MESH.Brokers.Mesh
{
    internal interface IMeshBroker
    {
        ValueTask<HttpResponseMessage> HandshakeAsync();

        ValueTask<HttpResponseMessage> SendMessageAsync(
            string mailboxTo,
            string workflowId,
            string message,
            string contentType);

        ValueTask<HttpResponseMessage> SendFileAsync(
            string mailboxTo,
            string workflowId,
            string contentType,
            byte[] fileContents,
            string fileName,
            string subject,
            string contentChecksum,
            string contentEncrypted);

        ValueTask<HttpResponseMessage> TrackMessageAsync(string messageId);
        ValueTask<HttpResponseMessage> GetMessagesAsync();
        ValueTask<HttpResponseMessage> GetMessageAsync(string messageId);
        ValueTask<HttpResponseMessage> AcknowledgeMessageAsync(string messageId);
    }
}
