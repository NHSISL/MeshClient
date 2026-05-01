// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Clients.Mailboxes
{
    /// <summary>
    /// Defines operations for interacting with a mailbox system.
    /// </summary>
    public interface IMailboxClient
    {
        /// <summary>
        /// Performs a handshake with the MESH mailbox service.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a value indicating whether the handshake was successful.
        /// </returns>
        ValueTask<bool> HandshakeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a file-based message to the specified recipient.
        /// </summary>
        /// <param name="mexTo">The recipient of the message.</param>
        /// <param name="mexWorkflowId">The workflow ID associated with the message.</param>
        /// <param name="content">The content stream to send.</param>
        /// <param name="mexSubject">The subject of the message (optional).</param>
        /// <param name="mexLocalId">A local identifier for the message (optional).</param>
        /// <param name="mexFileName">The file name associated with the message (optional).</param>
        /// <param name="mexContentChecksum">The checksum of the message content (optional).</param>
        /// <param name="contentType">The MIME type of the file content (default: "application/octet-stream").</param>
        /// <param name="contentEncoding">The encoding of the file content (optional).</param>
        /// <param name="accept">The expected response format (default: "application/json").</param>
        /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sent message.
        /// </returns>
        ValueTask<Message> SendMessageAsync(
            string mexTo,
            string mexWorkflowId,
            Stream content,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "application/octet-stream",
            string contentEncoding = "",
            string accept = "application/json",
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Tracks the status of a specific message.
        /// </summary>
        /// <param name="messageId">The identifier of the message to track.</param>
        /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the tracked message.
        /// </returns>
        ValueTask<Message> TrackMessageAsync(
            string messageId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of all available message identifiers.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
        ValueTask<List<string>> RetrieveMessagesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a specific message, with the message content written to the provided output stream.
        /// </summary>
        /// <param name="messageId">The identifier of the message to retrieve.</param>
        /// <param name="outputStream">An empty writable stream to which the message content will be written.</param>
        /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the retrieved message.
        /// </returns>
        ValueTask<Message> RetrieveMessageAsync(
            string messageId,
            Stream outputStream,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Acknowledges a specific message, marking it as processed.
        /// </summary>
        /// <param name="messageId">The identifier of the message to acknowledge.</param>
        /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a value indicating whether the acknowledgment was successful.
        /// </returns>
        ValueTask<bool> AcknowledgeMessageAsync(
            string messageId,
            CancellationToken cancellationToken = default);
    }
}
