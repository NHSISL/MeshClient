// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Brokers.DateTimes;
using NEL.MESH.Brokers.Identifiers;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal class MeshOrchestrationService : IMeshOrchestrationService
    {
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public MeshOrchestrationService(IDateTimeBroker dateTimeBroker, IIdentifierBroker identifierBroker)
        {
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<bool> AcknowledgeMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> HandshakeAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<Message> RetrieveMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> RetrieveMessagesAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<Message> SendFileAsync(Message message) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> SendMessageAsync(Message message) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            throw new System.NotImplementedException();
    }
}
