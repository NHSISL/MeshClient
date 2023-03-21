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
    internal partial class MeshService : IMeshService
    {
        private readonly IMeshBroker meshBroker;

        public MeshService(IMeshBroker meshBroker)
        {
            this.meshBroker = meshBroker;
        }

        public ValueTask<bool> HandshakeAsync() =>
            TryCatch(async () =>
            {
                HttpResponseMessage response = await this.meshBroker.HandshakeAsync();
                ValidateResponse(response);

                return true;
            });

        public ValueTask<Message> SendMessageAsync(Message message) =>
                throw new System.NotImplementedException();

        public ValueTask<Message> SendFileAsync(Message message) =>
                HttpResponseMessage response = await this.meshBroker.SendFileAsync();

        public ValueTask<List<string>> GetMessagesAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<Message> GetMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> AcknowledgeMessageAsync(string messageId) =>
            throw new System.NotImplementedException();
    }
}
