// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using NHSISL.MESH.Models.Foundations.Mesh;

namespace NHSISL.MESH.Clients.Mailboxes
{
    public static class ComposeMessage
    {
        public static Message CreateStringMessage(
            string mexTo,
            string mexWorkflowId,
            string content,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "",
            string contentEncoding = "",
            string accept = "application/json")
        {
            byte[] fileContent = Encoding.ASCII.GetBytes(content);

            return CreateFileMessage(
                mexTo,
                mexWorkflowId,
                fileContent,
                mexSubject,
                mexLocalId,
                mexFileName,
                mexContentChecksum,
                contentType,
                contentEncoding,
                accept);
        }

        public static Message CreateFileMessage(
            string mexTo,
            string mexWorkflowId,
            byte[] fileContent,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "",
            string contentEncoding = "",
            string accept = "application/json")
        {
            Message message = new Message();
            message.Headers.Add("mex-to", new List<string> { mexTo });
            message.Headers.Add("mex-workflowid", new List<string> { mexWorkflowId });
            message.FileContent = fileContent;

            if (!string.IsNullOrWhiteSpace(mexSubject))
            {
                message.Headers.Add("mex-subject", new List<string> { mexSubject });
            }

            if (!string.IsNullOrWhiteSpace(mexLocalId))
            {
                message.Headers.Add("mex-localid", new List<string> { mexLocalId });
            }

            if (!string.IsNullOrWhiteSpace(mexFileName))
            {
                message.Headers.Add("mex-filename", new List<string> { mexFileName });
            }

            if (!string.IsNullOrWhiteSpace(mexContentChecksum))
            {
                message.Headers.Add("mex-content-checksum", new List<string> { mexContentChecksum });
            }

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                message.Headers.Add("content-type", new List<string> { contentType });
            }

            if (!string.IsNullOrWhiteSpace(contentEncoding))
            {
                message.Headers.Add("content-encoding", new List<string> { contentEncoding });
            }

            if (!string.IsNullOrWhiteSpace(contentEncoding))
            {
                message.Headers.Add("accept", new List<string> { accept });
            }

            return message;
        }
    }
}
