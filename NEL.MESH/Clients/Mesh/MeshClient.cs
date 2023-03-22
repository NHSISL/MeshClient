// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using NEL.MESH.Services.Mesh;
using Xeptions;

namespace NEL.MESH.Clients.Mesh
{
    internal class MeshClient : IMeshClient
    {
        private readonly IMeshService meshService;

        public MeshClient(IMeshService meshService) =>
            this.meshService = meshService;

        public async ValueTask<bool> HandshakeAsync()
        {
            try
            {
                return await this.meshService.HandshakeAsync();
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

        public async ValueTask<Message> SendMessageAsync(Message message)
        {
            try
            {
                return await this.meshService.SendMessageAsync(message);
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

        public async ValueTask<Message> SendFileAsync(Message message)
        {
            try
            {
                return await this.meshService.SendFileAsync(message);
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

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> GetMessagesAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<Message> GetMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> AcknowledgeMessageAsync(string messageId) =>
            throw new System.NotImplementedException();
    }
}
