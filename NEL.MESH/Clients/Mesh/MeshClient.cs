// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Services.Mesh;

namespace NEL.MESH.Clients.Mesh
{
    internal class MeshClient : IMeshClient
    {
        private readonly IMeshService meshService;

        public MeshClient(IMeshService meshService) =>
            this.meshService = meshService;

        public ValueTask<bool> HandshakeAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<Message> SendMessageAsync(Message message) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> SendFileAsync(Message message) =>
            throw new System.NotImplementedException();

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
