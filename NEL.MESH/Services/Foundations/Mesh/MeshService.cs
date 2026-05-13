// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.ExternalModels;
using Newtonsoft.Json;

namespace NEL.MESH.Services.Foundations.Mesh
{
    internal partial class MeshService : IMeshService
    {
        private readonly IMeshBroker meshBroker;

        public MeshService(IMeshBroker meshBroker)
        {
            this.meshBroker = meshBroker;
        }

        public ValueTask<bool> HandshakeAsync(
            string authorizationToken,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateOnHandshake(authorizationToken);

                using HttpResponseMessage response =
                    await this.meshBroker.HandshakeAsync(authorizationToken, cancellationToken);

                await ValidateResponseAsync(response, cancellationToken);

                return response.IsSuccessStatusCode;
            });

        public ValueTask<Message> SendMessageAsync(
            Message message,
            byte[] fileContent,
            string authorizationToken,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateMeshMessageOnSendMessage(message, fileContent, authorizationToken);

                string chunkRange = GetKeyStringValue("mex-chunk-range", message.Headers)
                    .Replace("{", string.Empty)
                        .Replace("}", string.Empty)
                            .Trim();

                if (string.IsNullOrWhiteSpace(chunkRange))
                {
                    chunkRange = "1";
                }

                string chunkPart = (chunkRange.Split(':'))[0];
                int chunkNumber;

                if (!int.TryParse(chunkPart, out chunkNumber))
                {
                    chunkNumber = 1;
                }

                HttpResponseMessage responseMessage;

                if (chunkNumber <= 1)
                {
                    responseMessage = await this.meshBroker.SendMessageAsync(
                        authorizationToken,
                        mexFrom: GetKeyStringValue("mex-from", message.Headers),
                        mexTo: GetKeyStringValue("mex-to", message.Headers),
                        mexWorkflowId: GetKeyStringValue("mex-workflowid", message.Headers),
                        mexChunkRange: GetKeyStringValue("mex-chunk-range", message.Headers),
                        mexSubject: GetKeyStringValue("mex-subject", message.Headers),
                        mexLocalId: GetKeyStringValue("mex-localid", message.Headers),
                        mexFileName: GetKeyStringValue("mex-filename", message.Headers),
                        mexContentChecksum: GetKeyStringValue("mex-content-checksum", message.Headers),
                        contentType: GetKeyStringValue("content-type", message.Headers),
                        contentEncoding: GetKeyStringValue("content-encoding", message.Headers),
                        accept: GetKeyStringValue("accept", message.Headers),
                        fileContents: fileContent,
                        cancellationToken);
                }
                else
                {
                    ValidateMessageId(message.MessageId);
                    ValidateMexChunkRangeOnMultiPartFile(message);

                    responseMessage = await this.meshBroker.SendMessageAsync(
                        authorizationToken,
                        mexFrom: GetKeyStringValue("mex-from", message.Headers),
                        mexTo: GetKeyStringValue("mex-to", message.Headers),
                        mexWorkflowId: GetKeyStringValue("mex-workflowid", message.Headers),
                        mexChunkRange: GetKeyStringValue("mex-chunk-range", message.Headers),
                        mexSubject: GetKeyStringValue("mex-subject", message.Headers),
                        mexLocalId: GetKeyStringValue("mex-localid", message.Headers),
                        mexFileName: GetKeyStringValue("mex-filename", message.Headers),
                        mexContentChecksum: GetKeyStringValue("mex-content-checksum", message.Headers),
                        contentType: GetKeyStringValue("content-type", message.Headers),
                        contentEncoding: GetKeyStringValue("content-encoding", message.Headers),
                        accept: GetKeyStringValue("accept", message.Headers),
                        fileContents: fileContent,
                        messageId: message.MessageId,
                        chunkNumber: chunkNumber.ToString(),
                        cancellationToken);
                }

                using (responseMessage)
                {
                    await ValidateResponseAsync(responseMessage, cancellationToken);

                    string responseMessageBody =
                        await responseMessage.Content.ReadAsStringAsync(cancellationToken);

                    Message outputMessage = new Message
                    {
                        MessageId =
                            (JsonConvert.DeserializeObject<SendMessageResponse>(responseMessageBody)).MessageId,
                    };

                    GetHeaderValues(responseMessage, outputMessage);

                    return outputMessage;
                }
            });

        public ValueTask<Message> TrackMessageAsync(
            string messageId,
            string authorizationToken,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateTrackMessageArguments(messageId, authorizationToken);

                using HttpResponseMessage responseMessage =
                    await this.meshBroker.TrackMessageAsync(messageId, authorizationToken, cancellationToken);

                await ValidateResponseAsync(responseMessage, cancellationToken);
                string responseMessageBody = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

                Message outputMessage = new Message
                {
                    MessageId = messageId,
                    TrackingInfo = MapTrackMessageResponseToTrackingInfo(
                        JsonConvert.DeserializeObject<TrackMessageResponse>(responseMessageBody)),
                };

                GetHeaderValues(responseMessage, outputMessage);

                return outputMessage;
            });

        public ValueTask<List<string>> RetrieveMessagesAsync(
            string authorizationToken,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateRetrieveMessagesArguments(authorizationToken);

                using HttpResponseMessage responseMessage =
                    await this.meshBroker.GetMessagesAsync(authorizationToken, cancellationToken);

                await ValidateResponseAsync(responseMessage, cancellationToken);

                string responseMessageBody =
                    await responseMessage.Content.ReadAsStringAsync(cancellationToken);

                GetMessagesResponse getMessagesResponse =
                    JsonConvert.DeserializeObject<GetMessagesResponse>(responseMessageBody);

                return getMessagesResponse?.Messages ?? new List<string>();
            });

        public ValueTask<Message> RetrieveMessageAsync(
            string messageId,
            string authorizationToken,
            int chunkPart = 1,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateRetrieveMessageArguments(messageId, authorizationToken);
                HttpResponseMessage initialResponse;

                if (chunkPart == 1)
                {
                    initialResponse =
                        await this.meshBroker.GetMessageAsync(messageId, authorizationToken, cancellationToken);
                }
                else
                {
                    initialResponse = await this.meshBroker.GetMessageAsync(
                        messageId: messageId,
                        chunkNumber: chunkPart.ToString(),
                        authorizationToken: authorizationToken,
                        cancellationToken);
                }

                ValidateNullResponse(initialResponse);

                Message firstMessage = new Message
                {
                    MessageId = messageId,
                };

                using (initialResponse)
                {
                    await ValidateResponseAsync(initialResponse, cancellationToken);

                    foreach (var header in initialResponse.Headers)
                    {
                        firstMessage.Headers.Add(header.Key.ToLower(), header.Value.ToList());
                    }

                    foreach (var header in initialResponse.Content.Headers)
                    {
                        firstMessage.Headers.Add(header.Key.ToLower(), header.Value.ToList());
                    }
                }

                return firstMessage;
            });

        public ValueTask<Message> RetrieveMessageAsync(
            string messageId,
            string authorizationToken,
            Stream outputStream,
            int chunkPart = 1,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateRetrieveMessageStreamArguments(messageId, authorizationToken, outputStream);
                HttpResponseMessage response;

                if (chunkPart == 1)
                {
                    response = await this.meshBroker.GetMessageAsync(messageId, authorizationToken, cancellationToken);
                }
                else
                {
                    response = await this.meshBroker.GetMessageAsync(
                        messageId: messageId,
                        chunkNumber: chunkPart.ToString(),
                        authorizationToken: authorizationToken,
                        cancellationToken);
                }

                ValidateNullResponse(response);

                using (response)
                {
                    await ValidateResponseAsync(response, cancellationToken);
                    await response.Content.CopyToAsync(outputStream, cancellationToken);

                    Message outputMessage = new Message
                    {
                        MessageId = messageId,
                    };

                    foreach (var header in response.Headers)
                    {
                        outputMessage.Headers.Add(header.Key.ToLower(), header.Value.ToList());
                    }

                    foreach (var header in response.Content.Headers)
                    {
                        outputMessage.Headers.Add(header.Key.ToLower(), header.Value.ToList());
                    }

                    return outputMessage;
                }
            });

        public ValueTask<bool> AcknowledgeMessageAsync(
            string messageId,
            string authorizationToken,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateAcknowledgeMessageArguments(messageId, authorizationToken);

                using HttpResponseMessage response =
                    await this.meshBroker.AcknowledgeMessageAsync(messageId, authorizationToken, cancellationToken);

                await ValidateResponseAsync(response, cancellationToken);

                return response.IsSuccessStatusCode;
            });

        private static void GetHeaderValues(HttpResponseMessage responseMessage, Message outputMessage)
        {
            foreach (var header in responseMessage.Content.Headers)
            {
                outputMessage.Headers
                    .Add(header.Key.ToLower(), header.Value.ToList());
            }
        }

        private TrackingInfo MapTrackMessageResponseToTrackingInfo(TrackMessageResponse trackMessageResponse)
        {
            TrackingInfo trackingInfo = new TrackingInfo
            {
                AddressType = trackMessageResponse.AddressType,
                Checksum = trackMessageResponse.Checksum,
                ChunkCount = trackMessageResponse.ChunkCount,
                CompressFlag = trackMessageResponse.CompressFlag,
                DownloadTimestamp = trackMessageResponse.DownloadTimestamp,
                DtsId = trackMessageResponse.DtsId,
                EncryptedFlag = trackMessageResponse.EncryptedFlag,
                ExpiryTime = trackMessageResponse.ExpiryTime,
                FileName = trackMessageResponse.FileName,
                FileSize = trackMessageResponse.FileSize,
                IsCompressed = trackMessageResponse.IsCompressed,
                LocalId = trackMessageResponse.LocalId,
                MeshRecipientOdsCode = trackMessageResponse.MeshRecipientOdsCode,
                MessageId = trackMessageResponse.MessageId,
                MessageType = trackMessageResponse.MessageType,
                PartnerId = trackMessageResponse.PartnerId,
                Recipient = trackMessageResponse.Recipient,
                RecipientName = trackMessageResponse.RecipientName,
                RecipientOrgCode = trackMessageResponse.RecipientOrgCode,
                RecipientSmtp = trackMessageResponse.RecipientSmtp,
                Sender = trackMessageResponse.Sender,
                SenderName = trackMessageResponse.SenderName,
                SenderOdsCode = trackMessageResponse.SenderOdsCode,
                SenderOrgCode = trackMessageResponse.SenderOrgCode,
                SenderSmtp = trackMessageResponse.SenderSmtp,
                Status = trackMessageResponse.Status,
                StatusSuccess = trackMessageResponse.StatusSuccess,
                UploadTimestamp = trackMessageResponse.UploadTimestamp,
                Version = trackMessageResponse.Version,
                WorkflowId = trackMessageResponse.WorkflowId
            };

            return trackingInfo;
        }

        private static string GetKeyStringValue(string key, Dictionary<string, List<string>> dictionary)
        {
            string value = dictionary.ContainsKey(key)
                ? dictionary[key]?.First()
                : string.Empty;

            return value;
        }

        private static string GetKeyStringValue(string key, HttpContentHeaders headers)
        {
            return headers.Contains(key)
                ? headers.GetValues(key)?.FirstOrDefault() ?? string.Empty
                : string.Empty;
        }
    }
}
