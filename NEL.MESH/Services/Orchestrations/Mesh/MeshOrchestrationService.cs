﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Services.Foundations.Chunks;
using NEL.MESH.Services.Foundations.Mesh;
using NEL.MESH.Services.Foundations.Tokens;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal partial class MeshOrchestrationService : IMeshOrchestrationService
    {
        private readonly ITokenService tokenService;
        private readonly IMeshService meshService;
        private readonly IChunkService chunkService;

        public MeshOrchestrationService(
            ITokenService tokenService,
            IMeshService meshService,
            IChunkService chunkService)
        {
            this.tokenService = tokenService;
            this.meshService = meshService;
            this.chunkService = chunkService;
        }

        public ValueTask<bool> HandshakeAsync() =>
            TryCatch(async () =>
            {
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);

                return await this.meshService.HandshakeAsync(authorizationToken: token);
            });

        public ValueTask<Message> SendMessageAsync(Message message) =>
            TryCatch(async () =>
            {
                ValidateMessageIsNotNull(message);
                List<Message> chunkedMessages = this.chunkService.SplitMessageIntoChunks(message);
                Message outputMessage = null;

                foreach (Message chunkedMessage in chunkedMessages)
                {
                    string token = await this.tokenService.GenerateTokenAsync();
                    ValidateToken(token);
                    chunkedMessage.MessageId = outputMessage?.MessageId;

                    Message sentMessage = await this.meshService
                        .SendMessageAsync(chunkedMessage, authorizationToken: token);

                    if (chunkedMessage == chunkedMessages.First())
                    {
                        outputMessage = sentMessage;
                        outputMessage.StringContent = message.StringContent;
                    }
                }

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

                Message outputMessage =
                    await this.meshService.RetrieveMessageAsync(messageId, authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<List<string>> RetrieveMessagesAsync() =>
            TryCatch(async () =>
            {
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);
                List<string> outputMessage = await this.meshService.RetrieveMessagesAsync(authorizationToken: token);

                return outputMessage;
            });

        public ValueTask<bool> AcknowledgeMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateTrackMessageArgs(messageId);
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);

                return await this.meshService.AcknowledgeMessageAsync(messageId, authorizationToken: token);
            });
    }
}
