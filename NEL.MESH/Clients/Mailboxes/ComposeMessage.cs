// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Clients.Mailboxes
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
            Message message = new Message();
            message.Headers.Add("Mex-To", new List<string> { mexTo });
            message.Headers.Add("Mex-WorkflowID", new List<string> { mexWorkflowId });
            message.StringContent = content;

            if (!string.IsNullOrWhiteSpace(mexSubject))
            {
                message.Headers.Add("Mex-Subject", new List<string> { mexSubject });
            }

            if (!string.IsNullOrWhiteSpace(mexFileName))
            {
                message.Headers.Add("Mex-FileName", new List<string> { mexFileName });
            }

            if (!string.IsNullOrWhiteSpace(mexContentChecksum))
            {
                message.Headers.Add("Mex-Content-Checksum", new List<string> { mexContentChecksum });
            }

            if (!string.IsNullOrWhiteSpace(mexLocalId))
            {
                message.Headers.Add("Mex-LocalID", new List<string> { mexLocalId });
            }

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                message.Headers.Add("Content-Type", new List<string> { contentType });
            }

            if (!string.IsNullOrWhiteSpace(contentEncoding))
            {
                message.Headers.Add("Content-Encoding", new List<string> { contentEncoding });
            }

            message.Headers.Add("Accept", new List<string> { accept });

            return message;
        }

        public static Message CreateFileMessage(
            string mexTo,
            string mexWorkflowId,
            byte[] fileContent,
            string mexContentEncrypted,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "",
            string contentEncoding = "",
            string accept = "application/json")
        {
            Message message = new Message();
            message.Headers.Add("Mex-To", new List<string> { mexTo });
            message.Headers.Add("Mex-WorkflowID", new List<string> { mexWorkflowId });
            message.FileContent = fileContent;
            message.Headers.Add("Mex-Content-Encrypted", new List<string> { mexContentEncrypted });

            if (!string.IsNullOrWhiteSpace(mexSubject))
            {
                message.Headers.Add("Mex-Subject", new List<string> { mexSubject });
            }

            if (!string.IsNullOrWhiteSpace(mexFileName))
            {
                message.Headers.Add("Mex-FileName", new List<string> { mexFileName });
            }

            if (!string.IsNullOrWhiteSpace(mexContentChecksum))
            {
                message.Headers.Add("Mex-Content-Checksum", new List<string> { mexContentChecksum });
            }

            if (!string.IsNullOrWhiteSpace(mexLocalId))
            {
                message.Headers.Add("Mex-LocalID", new List<string> { mexLocalId });
            }

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                message.Headers.Add("Content-Type", new List<string> { contentType });
            }

            if (!string.IsNullOrWhiteSpace(contentEncoding))
            {
                message.Headers.Add("Content-Encoding", new List<string> { contentEncoding });
            }

            message.Headers.Add("Accept", new List<string> { accept });

            return message;
        }
    }
}
