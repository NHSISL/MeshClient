// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using NEL.MESH.Services.Orchestrations.Mesh;
using Xeptions;

namespace NEL.MESH.Clients.Mailboxes
{
    internal class MailboxClient : IMailboxClient
    {
        private readonly IMeshOrchestrationService meshOrchestrationService;

        public MailboxClient(IMeshOrchestrationService meshOrchestrationService) =>
            this.meshOrchestrationService = meshOrchestrationService;

        public async ValueTask<bool> HandshakeAsync()
        {
            try
            {
                return await meshOrchestrationService.HandshakeAsync();
            }
            catch (MeshValidationException meshValidationException)
            {
                throw new MeshClientValidationException(
                    meshValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshDependencyValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshDependencyException.InnerException as Xeption);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw new MeshClientServiceException(
                    meshServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<Message> SendMessageAsync(
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
            try
            {
                Message message = ComposeMessage.CreateStringMessage(
                    mexTo,
                    mexWorkflowId,
                    content,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept);

                return await meshOrchestrationService.SendMessageAsync(message);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw new MeshClientValidationException(
                    meshValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshDependencyValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshDependencyException.InnerException as Xeption);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw new MeshClientServiceException(
                    meshServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<Message> SendMessageAsync(
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
            try
            {
                Message message = ComposeMessage.CreateFileMessage(
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

                return await meshOrchestrationService.SendMessageAsync(message);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw new MeshClientValidationException(
                    meshValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshDependencyValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshDependencyException.InnerException as Xeption);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw new MeshClientServiceException(
                    meshServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<Message> TrackMessageAsync(string messageId)
        {
            try
            {
                return await meshOrchestrationService.TrackMessageAsync(messageId);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw new MeshClientValidationException(
                    meshValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshDependencyValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshDependencyException.InnerException as Xeption);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw new MeshClientServiceException(
                    meshServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<List<string>> RetrieveMessagesAsync()
        {
            try
            {
                return await meshOrchestrationService.RetrieveMessagesAsync();
            }
            catch (MeshValidationException meshValidationException)
            {
                throw new MeshClientValidationException(
                    meshValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshDependencyValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshDependencyException.InnerException as Xeption);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw new MeshClientServiceException(
                    meshServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<Message> RetrieveMessageAsync(string messageId)
        {
            try
            {
                return await meshOrchestrationService.RetrieveMessageAsync(messageId);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw new MeshClientValidationException(
                    meshValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshDependencyValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshDependencyException.InnerException as Xeption);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw new MeshClientServiceException(
                    meshServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<bool> AcknowledgeMessageAsync(string messageId)
        {
            try
            {
                return await meshOrchestrationService.AcknowledgeMessageAsync(messageId);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw new MeshClientValidationException(
                    meshValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshDependencyValidationException.InnerException as Xeption);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshDependencyException.InnerException as Xeption);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw new MeshClientServiceException(
                    meshServiceException.InnerException as Xeption);
            }
        }
    }
}
