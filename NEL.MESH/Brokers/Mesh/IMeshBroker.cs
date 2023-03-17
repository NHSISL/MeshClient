// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;

namespace NEL.MESH.Brokers.Mesh
{
    internal interface IMeshBroker
    {
        Task<HttpResponseMessage> HandshakeAsync();
        Task<HttpResponseMessage> SendMessageAsync(string message, string mailboxTo, string workflowId);
        Task<HttpResponseMessage> SendFileAsync(byte[] fileContents, string mailboxTo, string workflowId);
        Task<HttpResponseMessage> TrackMessageAsync(string messageId);
        Task<HttpResponseMessage> GetMessagesAsync();
        Task<HttpResponseMessage> GetMessageAsync(string messageId);
        Task<HttpResponseMessage> AcknowledgeMessageAsync(string messageId);
    }
}
