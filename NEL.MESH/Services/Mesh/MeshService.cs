// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

                return true;
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
                throw new System.NotImplementedException();

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

        public ValueTask<List<string>> GetMessagesAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<Message> GetMessageAsync(string messageId) =>
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

             return true;
         });

        private static void GetHeaderValues(HttpResponseMessage responseMessage, Message outputMessage)
        {
            foreach (var header in responseMessage.Content.Headers)
            {
                outputMessage.Headers.Add(header.Key, header.Value.ToList());
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
    }
}
