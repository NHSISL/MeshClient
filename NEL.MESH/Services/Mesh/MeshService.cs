// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.ExternalModeld;
using Newtonsoft.Json;

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

                return response.IsSuccessStatusCode;
            });

        public ValueTask<Message> SendMessageAsync(Message message) =>
            TryCatch(async () =>
            {
                ValidateMeshMessageOnSendMessage(message);

                HttpResponseMessage responseMessage = await this.meshBroker.SendMessageAsync(
                    mailboxTo: message.Headers["Mex-To"].First(),
                    workflowId: message.Headers["Mex-WorkflowID"].First(),
                    stringConent: message.StringContent,
                    contentType: message.Headers["Content-Type"].First(),
                    localId: message.Headers["Mex-LocalID"].First(),
                    subject: message.Headers["Mex-Subject"].First(),
                    contentEncrypted: message.Headers["Mex-Content-Encrypted"].First()
                    );

                ValidateResponse(responseMessage);
                string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;

                Message outputMessage = new Message
                {
                    MessageId = (JsonConvert.DeserializeObject<SendMessageResponse>(responseMessageBody)).MessageId,
                    StringContent = responseMessageBody,
                };

                GetHeaderValues(responseMessage, outputMessage);

                return outputMessage;
            });

        public ValueTask<Message> SendFileAsync(Message message) =>
            TryCatch(async () =>
            {
                ValidateMeshMessageOnSendFile(message);
                HttpResponseMessage responseFileMessage = await this.meshBroker.SendFileAsync(
                        mailboxTo: GetKeyStringValue("Mex-To", message.Headers),
                        workflowId: GetKeyStringValue("Mex-WorkflowID", message.Headers),
                        contentType: GetKeyStringValue("Content-Type", message.Headers),
                        fileContents: message.FileContent,
                        fileName: GetKeyStringValue("Mex-FileName", message.Headers),
                        subject: GetKeyStringValue("Mex-Subject", message.Headers),
                        contentChecksum: GetKeyStringValue("Mex-Content-Checksum", message.Headers),
                        contentEncrypted: GetKeyStringValue("Mex-Content-Encrypted", message.Headers),
                        encoding: GetKeyStringValue("Mex-Encoding", message.Headers),
                        chunkRange: GetKeyStringValue("Mex-Chunk-Range", message.Headers),
                        localId: GetKeyStringValue("Mex-LocalID", message.Headers)
                        );

                string responseMessageBody = responseFileMessage.Content.ReadAsStringAsync().Result;

                Message outputMessage = new Message
                {
                    MessageId = (JsonConvert.DeserializeObject<SendMessageResponse>(responseMessageBody)).MessageId,
                    StringContent = responseMessageBody,
                };

                GetHeaderValues(responseFileMessage, outputMessage);

                return outputMessage;
            });

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateTrackMessageArguments(messageId);
                HttpResponseMessage responseMessage = await this.meshBroker.TrackMessageAsync(messageId);
                ValidateResponse(responseMessage);
                string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;

                Message outputMessage = new Message
                {
                    MessageId = messageId,
                    TrackingInfo = MapTrackMessageResponseToTrackingInfo(
                        JsonConvert.DeserializeObject<TrackMessageResponse>(responseMessageBody)),
                };

                GetHeaderValues(responseMessage, outputMessage);

                return outputMessage;
            });

        public ValueTask<List<string>> RetrieveMessagesAsync() =>
            TryCatch(async () =>
            {
                HttpResponseMessage responseMessage = await this.meshBroker.GetMessagesAsync();
                ValidateResponse(responseMessage);
                string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;

                GetMessagesResponse getMessagesResponse =
                    JsonConvert.DeserializeObject<GetMessagesResponse>(responseMessageBody);

                return getMessagesResponse.Messages;
            });

        public ValueTask<Message> RetrieveMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateTrackMessageArguments(messageId);
                HttpResponseMessage responseMessage = await this.meshBroker.GetMessageAsync(messageId);
                ValidateResponse(responseMessage);
                string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;

                Message outputMessage = new Message
                {
                    MessageId = messageId,
                    StringContent = responseMessageBody,
                };

                foreach (var header in responseMessage.Content.Headers)
                {
                    outputMessage.Headers.Add(header.Key, header.Value.ToList());
                }

                return outputMessage;
            });

        public ValueTask<bool> AcknowledgeMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateTrackMessageArguments(messageId);
                HttpResponseMessage response = await this.meshBroker.AcknowledgeMessageAsync(messageId);
                ValidateResponse(response);

                return response.IsSuccessStatusCode;
            });

        private static void GetHeaderValues(HttpResponseMessage responseMessage, Message outputMessage)
        {
            foreach (var header in responseMessage.Content.Headers)
            {
                outputMessage.Headers
                    .Add(header.Key, header.Value.ToList());
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
            return dictionary.ContainsKey(key)
                ? dictionary[key]?.First()
                : string.Empty;
        }
    }
}
