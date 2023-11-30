// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NEL.MESH.Brokers.Mesh;
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
        private readonly IMeshConfigurationBroker meshConfigurationBroker;

        public MeshOrchestrationService(
            ITokenService tokenService,
            IMeshService meshService,
            IChunkService chunkService,
            IMeshConfigurationBroker meshConfigurationBroker)
        {
            this.tokenService = tokenService;
            this.meshService = meshService;
            this.chunkService = chunkService;
            this.meshConfigurationBroker = meshConfigurationBroker;
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
                SetHeader(message, "mex-from", this.meshConfigurationBroker.MexFrom);
                List<Message> chunkedMessages = this.chunkService.SplitMessageIntoChunks(message);
                ValidateChunksOnSendMessage(chunkedMessages);
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
                        outputMessage.FileContent = message.FileContent;
                    }
                }

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
                    await this.meshService.RetrieveMessageAsync(messageId, authorizationToken: token, 1);

                var chunks = outputMessage.Headers
                    .FirstOrDefault(h => h.Key == "mex-chunk-range")
                    .Value?
                    .FirstOrDefault();

                if (chunks != null)
                {
                    string chunkRange = chunks.Replace("{", string.Empty).Replace("}", string.Empty);
                    string[] parts = chunkRange.Split(":");
                    int totalChunks = int.Parse(parts[1]);

                    for (int chunkId = 2; chunkId <= totalChunks; chunkId++)
                    {
                        token = await this.tokenService.GenerateTokenAsync();

                        Message responseMessage =
                            await this.meshService.RetrieveMessageAsync(messageId, authorizationToken: token, chunkId);

                        byte[] fileContent = responseMessage.FileContent;
                        outputMessage.FileContent = outputMessage.FileContent.Concat(fileContent).ToArray();
                    }
                }

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

        private static void SetHeader(Message message, string key, string value)
        {
            if (message.Headers.ContainsKey(key))
            {
                message.Headers[key] = new List<string> { value };
            }
            else
            {
                message.Headers.Add(key, new List<string> { value });
            }
        }
    }
}
