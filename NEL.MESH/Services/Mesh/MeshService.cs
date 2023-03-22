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

                foreach (var header in responseMessage.Content.Headers)
                {
                    outputMessage.Headers.Add(header.Key, header.Value.ToList());
                }

                return outputMessage;
            });

        public ValueTask<Message> SendFileAsync(Message message) =>
                throw new System.NotImplementedException();

        public ValueTask<List<string>> GetMessagesAsync() =>
            throw new System.NotImplementedException();

        public async ValueTask<Message> GetMessageAsync(string messageId)
        {
            HttpResponseMessage responseMessage = await this.meshBroker.GetMessageAsync(messageId);

            string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;

            Message outputMessage = new Message
            {
                MessageId = (JsonConvert.DeserializeObject<SendMessageResponse>(responseMessageBody)).MessageId,
                StringContent = responseMessageBody,
            };

            foreach (var header in responseMessage.Content.Headers)
            {
                outputMessage.Headers.Add(header.Key, header.Value.ToList());
            }

            return outputMessage;
        }

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> AcknowledgeMessageAsync(string messageId) =>
            throw new System.NotImplementedException();
    }
}
