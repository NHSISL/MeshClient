﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Services.Foundations.Mesh;
using NEL.MESH.Services.Foundations.Tokens;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal class MeshOrchestrationService : IMeshOrchestrationService
    {
        private readonly ITokenService tokenService;
        private readonly IMeshService meshService;

        public MeshOrchestrationService(ITokenService tokenService, IMeshService meshService)
        {
            this.tokenService = tokenService;
            this.meshService = meshService;
        }

        public ValueTask<bool> HandshakeAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<Message> SendMessageAsync(Message message) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> SendFileAsync(Message message) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> RetrieveMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> RetrieveMessagesAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<bool> AcknowledgeMessageAsync(string messageId) =>
            throw new System.NotImplementedException();
    }
}