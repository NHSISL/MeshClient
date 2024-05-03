// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NHSISL.MESH.Models.Foundations.Mesh;

namespace NHSISL.MESH.Clients.Mailboxes
{
    public interface IMailboxClient
    {
        ValueTask<bool> HandshakeAsync();

        ValueTask<Message> SendMessageAsync(
            string mexTo,
            string mexWorkflowId,
            string content,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "",
            string contentEncoding = "",
            string accept = "application/json");

        ValueTask<Message> SendMessageAsync(
            string mexTo,
            string mexWorkflowId,
            byte[] fileContent,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "application/octet-stream",
            string contentEncoding = "",
            string accept = "application/json");

        ValueTask<Message> TrackMessageAsync(string messageId);
        ValueTask<List<string>> RetrieveMessagesAsync();
        ValueTask<Message> RetrieveMessageAsync(string messageId);
        ValueTask<bool> AcknowledgeMessageAsync(string messageId);
    }
}
