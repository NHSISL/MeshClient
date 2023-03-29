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
                ValidateToken(token);
                Message outputMessage = await this.meshService.TrackMessageAsync(messageId, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<Message> RetrieveMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateTrackMessageArgs(messageId);
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);
                Message outputMessage = await this.meshService.RetrieveMessageAsync(messageId, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<List<string>> RetrieveMessagesAsync() =>
            throw new System.NotImplementedException();

        public async ValueTask<bool> AcknowledgeMessageAsync(string messageId)
        {
            string token = await this.tokenService.GenerateTokenAsync();
            bool x = await this.meshService.AcknowledgeMessageAsync(messageId, authorizationToken: token);
            return x;
        }
    }
}
