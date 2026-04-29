// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
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
            catch (MeshOrchestrationValidationException meshOrchestrationValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationValidationException.InnerException as Xeption);
            }
            catch (MeshOrchestrationDependencyValidationException meshOrchestrationDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (MeshOrchestrationDependencyException meshOrchestrationDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (MeshOrchestrationServiceException meshOrchestrationServiceException)
            {
                throw new MeshClientServiceException(
                    meshOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<Message> SendMessageAsync(
            string mexTo,
            string mexWorkflowId,
            Stream content,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "application/octet-stream",
            string contentEncoding = "",
            string accept = "application/json")
        {
            try
            {
                Message message = ComposeMessage.CreateMessage(
                    mexTo,
                    mexWorkflowId,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept);

                return await meshOrchestrationService.SendMessageAsync(message, content);
            }
            catch (MeshOrchestrationValidationException meshOrchestrationValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationValidationException.InnerException as Xeption,
                    meshOrchestrationValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyValidationException meshOrchestrationDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationDependencyValidationException.InnerException as Xeption,
                    meshOrchestrationDependencyValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyException meshOrchestrationDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (MeshOrchestrationServiceException meshOrchestrationServiceException)
            {
                throw new MeshClientServiceException(
                    meshOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<Message> TrackMessageAsync(string messageId)
        {
            try
            {
                return await meshOrchestrationService.TrackMessageAsync(messageId);
            }
            catch (MeshOrchestrationValidationException meshOrchestrationValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationValidationException.InnerException as Xeption,
                    meshOrchestrationValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyValidationException meshOrchestrationDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationDependencyValidationException.InnerException as Xeption,
                    meshOrchestrationDependencyValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyException meshOrchestrationDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (MeshOrchestrationServiceException meshOrchestrationServiceException)
            {
                throw new MeshClientServiceException(
                    meshOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<List<string>> RetrieveMessagesAsync()
        {
            try
            {
                return await meshOrchestrationService.RetrieveMessagesAsync();
            }
            catch (MeshOrchestrationValidationException meshOrchestrationValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationValidationException.InnerException as Xeption,
                    meshOrchestrationValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyValidationException meshOrchestrationDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationDependencyValidationException.InnerException as Xeption,
                    meshOrchestrationDependencyValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyException meshOrchestrationDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (MeshOrchestrationServiceException meshOrchestrationServiceException)
            {
                throw new MeshClientServiceException(
                    meshOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<Message> RetrieveMessageAsync(string messageId, Stream outputStream)
        {
            try
            {
                return await meshOrchestrationService.RetrieveMessageAsync(messageId, outputStream);
            }
            catch (MeshOrchestrationValidationException meshOrchestrationValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationValidationException.InnerException as Xeption,
                    meshOrchestrationValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyValidationException meshOrchestrationDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationDependencyValidationException.InnerException as Xeption,
                    meshOrchestrationDependencyValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyException meshOrchestrationDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (MeshOrchestrationServiceException meshOrchestrationServiceException)
            {
                throw new MeshClientServiceException(
                    meshOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<bool> AcknowledgeMessageAsync(string messageId)
        {
            try
            {
                return await meshOrchestrationService.AcknowledgeMessageAsync(messageId);
            }
            catch (MeshOrchestrationValidationException meshOrchestrationValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationValidationException.InnerException as Xeption,
                    meshOrchestrationValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyValidationException meshOrchestrationDependencyValidationException)
            {
                throw new MeshClientValidationException(
                    meshOrchestrationDependencyValidationException.InnerException as Xeption,
                    meshOrchestrationDependencyValidationException.InnerException.Data);
            }
            catch (MeshOrchestrationDependencyException meshOrchestrationDependencyException)
            {
                throw new MeshClientDependencyException(
                    meshOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (MeshOrchestrationServiceException meshOrchestrationServiceException)
            {
                throw new MeshClientServiceException(
                    meshOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
