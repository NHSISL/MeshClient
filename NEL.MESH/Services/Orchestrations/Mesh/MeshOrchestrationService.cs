// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Services.Foundations.Mesh;
using NEL.MESH.Services.Foundations.Tokens;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal partial class MeshOrchestrationService : IMeshOrchestrationService
    {
        private readonly ITokenService tokenService;
        private readonly IMeshService meshService;

        public MeshOrchestrationService(ITokenService tokenService, IMeshService meshService)
        {
            this.tokenService = tokenService;
            this.meshService = meshService;
        }

        public ValueTask<bool> HandshakeAsync() =>
            TryCatch(async () =>
            {
                return await this.meshService.HandshakeAsync();
            });

        public ValueTask<Message> SendMessageAsync(Message message) =>
            TryCatch(async () =>
            {
                ValidateMessageIsNotNull(message);
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);
                Message outputMessage = await this.meshService.SendMessageAsync(message, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<Message> SendFileAsync(Message message) =>
            TryCatch(async () =>
            {
                ValidateMessageIsNotNull(message);
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);
                Message outputMessage = await this.meshService.SendFileAsync(message, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateTrackMessageArgs(messageId);
                string token = await this.tokenService.GenerateTokenAsync();
                Message outputMessage = await this.meshService.TrackMessageAsync(messageId, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<Message> RetrieveMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> RetrieveMessagesAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<bool> AcknowledgeMessageAsync(string messageId) =>
            throw new System.NotImplementedException();
    }
}
