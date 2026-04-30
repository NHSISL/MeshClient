// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
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

        public async ValueTask<bool> HandshakeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await meshOrchestrationService.HandshakeAsync(cancellationToken);
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
            string accept = "application/json",
            CancellationToken cancellationToken = default)
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

                return await meshOrchestrationService.SendMessageAsync(message, content, cancellationToken);
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

        public async ValueTask<Message> TrackMessageAsync(
            string messageId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await meshOrchestrationService.TrackMessageAsync(messageId, cancellationToken);
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

        public async ValueTask<List<string>> RetrieveMessagesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await meshOrchestrationService.RetrieveMessagesAsync(cancellationToken);
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

        public async ValueTask<Message> RetrieveMessageAsync(
            string messageId,
            Stream outputStream,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await meshOrchestrationService.RetrieveMessageAsync(messageId, outputStream, cancellationToken);
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

        public async ValueTask<bool> AcknowledgeMessageAsync(
            string messageId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await meshOrchestrationService.AcknowledgeMessageAsync(messageId, cancellationToken);
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
