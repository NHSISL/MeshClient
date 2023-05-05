// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Services.Foundations.Mesh;

namespace NEL.MESH.Services.Processings.Mesh
{
    internal partial class MeshProcessingService : IMeshProcessingService
    {
        private readonly IMeshService meshService;

        public MeshProcessingService(IMeshService meshService)
        {
            this.meshService = meshService;
        }

        public ValueTask<bool> HandshakeAsync(string authorizationToken) =>
            TryCatch(async () =>
            {
                ValidateOnHandshake(authorizationToken);

                return await this.meshService.HandshakeAsync(authorizationToken);
            });

        public ValueTask<Message> SendMessageAsync(Message message, string authorizationToken) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> SendFileAsync(Message message, string authorizationToken) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> TrackMessageAsync(string messageId, string authorizationToken) =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> RetrieveMessagesAsync(string authorizationToken) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> RetrieveMessageAsync(string messageId, string authorizationToken) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> AcknowledgeMessageAsync(string messageId, string authorizationToken) =>
            throw new System.NotImplementedException();
    }
}
