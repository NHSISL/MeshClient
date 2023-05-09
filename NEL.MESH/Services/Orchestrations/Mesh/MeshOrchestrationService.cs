// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Services.Processings.Mesh;
using NEL.MESH.Services.Processings.Tokens;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal partial class MeshOrchestrationService : IMeshOrchestrationService
    {
        private readonly ITokenProcessingService tokenProcessingService;
        private readonly IMeshProcessingService meshProcessingService;

        public MeshOrchestrationService(
            ITokenProcessingService tokenProcessingService,
            IMeshProcessingService meshProcessingService)
        {
            this.tokenProcessingService = tokenProcessingService;
            this.meshProcessingService = meshProcessingService;
        }

        public ValueTask<bool> HandshakeAsync() =>
            TryCatch(async () =>
            {
                string token = await this.tokenProcessingService.GenerateTokenAsync();
                ValidateToken(token);

                return await this.meshProcessingService.HandshakeAsync(authorizationToken: token);
            });

        public ValueTask<Message> SendMessageAsync(Message message) =>
            TryCatch(async () =>
            {
                ValidateMessageIsNotNull(message);
                string token = await this.tokenProcessingService.GenerateTokenAsync();
                ValidateToken(token);

                Message outputMessage =
                    await this.meshProcessingService.SendMessageAsync(message, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<Message> SendFileAsync(Message message) =>
            TryCatch(async () =>
            {
                ValidateMessageIsNotNull(message);
                string token = await this.tokenProcessingService.GenerateTokenAsync();
                ValidateToken(token);

                Message outputMessage =
                    await this.meshProcessingService.SendFileAsync(message, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateTrackMessageArgs(messageId);
                string token = await this.tokenProcessingService.GenerateTokenAsync();
                ValidateToken(token);

                Message outputMessage =
                    await this.meshProcessingService.TrackMessageAsync(messageId, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<Message> RetrieveMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateTrackMessageArgs(messageId);
                string token = await this.tokenProcessingService.GenerateTokenAsync();
                ValidateToken(token);

                Message outputMessage =
                    await this.meshProcessingService.RetrieveMessageAsync(messageId, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<List<string>> RetrieveMessagesAsync() =>
            TryCatch(async () =>
            {
                string token = await this.tokenProcessingService.GenerateTokenAsync();
                ValidateToken(token);
                List<string> outputMessage = await this.meshProcessingService.RetrieveMessagesAsync(authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<bool> AcknowledgeMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateTrackMessageArgs(messageId);
                string token = await this.tokenProcessingService.GenerateTokenAsync();
                ValidateToken(token);

                return await this.meshProcessingService.AcknowledgeMessageAsync(messageId, authorizationToken: token);
            });
    }
}
