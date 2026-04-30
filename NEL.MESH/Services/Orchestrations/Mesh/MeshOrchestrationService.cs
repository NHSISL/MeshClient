// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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

        public ValueTask<bool> HandshakeAsync(CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);

                return await this.meshService.HandshakeAsync(authorizationToken: token, cancellationToken);
            });

        public ValueTask<Message> SendMessageAsync(
            Message message,
            Stream content,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateMessageIsNotNull(message);
                ValidateSendMessageStreamArgs(message, content);
                SetHeader(message, "mex-from", this.meshConfigurationBroker.MexFrom);

                IEnumerable<(Message message, byte[] content)> chunkedMessages =
                    this.chunkService.SplitStreamIntoChunks(message, content);

                Message outputMessage = null;

                foreach ((Message chunkedMessage, byte[] chunkContent) in chunkedMessages)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string token = await this.tokenService.GenerateTokenAsync();
                    ValidateToken(token);
                    chunkedMessage.MessageId = outputMessage?.MessageId;

                    Message sentMessage = await this.meshService
                        .SendMessageAsync(chunkedMessage, chunkContent, authorizationToken: token, cancellationToken);

                    if (outputMessage is null)
                    {
                        outputMessage = sentMessage;
                    }
                }

                if (outputMessage is null)
                {
                    ValidateChunksOnSendMessage(new List<Message>());
                }

                return outputMessage;
            });

        public ValueTask<Message> TrackMessageAsync(
            string messageId,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateTrackMessageArgs(messageId);
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);

                Message outputMessage =
                    await this.meshService.TrackMessageAsync(messageId, authorizationToken: token, cancellationToken);

                return outputMessage;
            });

        public ValueTask<Message> RetrieveMessageAsync(
            string messageId,
            Stream content,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateRetrieveMessageArgs(messageId, content);
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);

                Message outputMessage =
                    await this.meshService.RetrieveMessageAsync(
                        messageId,
                        authorizationToken: token,
                        outputStream: content,
                        chunkPart: 1,
                        cancellationToken);

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
                        cancellationToken.ThrowIfCancellationRequested();
                        token = await this.tokenService.GenerateTokenAsync();
                        ValidateToken(token);

                        await this.meshService.RetrieveMessageAsync(
                            messageId,
                            authorizationToken: token,
                            outputStream: content,
                            chunkPart: chunkId,
                            cancellationToken);
                    }
                }

                return outputMessage;
            });

        public ValueTask<List<string>> RetrieveMessagesAsync(CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);

                List<string> outputMessage =
                    await this.meshService.RetrieveMessagesAsync(authorizationToken: token, cancellationToken);

                return outputMessage;
            });

        public ValueTask<bool> AcknowledgeMessageAsync(
            string messageId,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateTrackMessageArgs(messageId);
                string token = await this.tokenService.GenerateTokenAsync();
                ValidateToken(token);

                return await this.meshService
                    .AcknowledgeMessageAsync(messageId, authorizationToken: token, cancellationToken);
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
