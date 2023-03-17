// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Mesh
{
    internal class MeshService : IMeshService
    {
        private readonly IMeshBroker meshBroker;

        public MeshService(IMeshBroker meshBroker)
        {
            this.meshBroker = meshBroker;
        }

        public async ValueTask<bool> HandshakeAsync()
        {
            HttpResponseMessage response = await this.meshBroker.HandshakeAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public ValueTask<Message> SendMessageAsync(Message message) =>
                throw new System.NotImplementedException();

        public ValueTask<Message> SendFileAsync(Message message) =>
                throw new System.NotImplementedException();

        public ValueTask<List<string>> GetMessagesAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<Message> GetMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<HttpResponseMessage> TrackMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<HttpResponseMessage> AcknowledgeMessageAsync(string messageId) =>
            throw new System.NotImplementedException();
    }
}
