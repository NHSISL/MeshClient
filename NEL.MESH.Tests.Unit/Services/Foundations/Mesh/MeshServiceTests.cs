// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Moq;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.ExternalModels;
using NEL.MESH.Services.Foundations.Mesh;
using Newtonsoft.Json;
using Tynamix.ObjectFiller;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        private readonly Mock<IMeshBroker> meshBrokerMock;
        private readonly IMeshService meshService;

        public MeshServiceTests()
        {
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.meshService = new MeshService(meshBroker: this.meshBrokerMock.Object);
        }

        public static TheoryData DependencyValidationResponseMessages()
        {
            var invalidValue =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "400"))
                {
                    ReasonPhrase = "Invalid Value"
                };

            var missingValue =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "400"))
                {
                    ReasonPhrase = "Invalid Value"
                };

            var accessDenied =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "401"))
                {
                    ReasonPhrase = "Access Denied"
                };

            var unauthorised =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "401"))
                {
                    ReasonPhrase = "Unauthorised"
                };

            var forbidden =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "403"))
                {
                    ReasonPhrase = "Forbidden"
                };

            var access_Denied =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "403"))
                {
                    ReasonPhrase = "Access Denied"
                };

            var resourceNotFound =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "404"))
                {
                    ReasonPhrase = "Unauthorised Value"
                };

            var tooManyRequests =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "429"))
                {
                    ReasonPhrase = "Too Many Requests"
                };

            return new TheoryData<HttpResponseMessage>
            {
                invalidValue,
                missingValue,
                accessDenied,
                unauthorised,
                forbidden,
                access_Denied,
                resourceNotFound,
                tooManyRequests
            };
        }

        public static TheoryData DependencyResponseMessages()
        {
            var internalServerError =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "500"))
                {
                    ReasonPhrase = "Internal Server Error"
                };

            var missingValue =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "501"))
                {
                    ReasonPhrase = "Not Implemented"
                };

            var accessDenied =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "502"))
                {
                    ReasonPhrase = "Bad Gateway"
                };

            var unauthorised =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "503"))
                {
                    ReasonPhrase = "Service Unavailable"
                };

            var forbidden =
                new HttpResponseMessage((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), "504"))
                {
                    ReasonPhrase = "gateway Timeout"
                };

            return new TheoryData<HttpResponseMessage>
            {
                internalServerError,
                missingValue,
                accessDenied,
                unauthorised,
                forbidden
            };
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetKeyStringValue(string key, Dictionary<string, List<string>> dictionary)
        {
            return dictionary.ContainsKey(key)
                ? dictionary[key]?.First()
                : string.Empty;
        }


        public static List<string> GetParts(string content, int parts)
        {
            if (string.IsNullOrEmpty(content) || parts <= 0)
            {
                return new List<string>(); // Return an empty list if the input string is null or empty or parts is less than or equal to 0.
            }

            if (parts > content.Length) // If parts is greater than the length of the content, adjust it to the length of the content.
            {
                parts = content.Length;
            }

            int length = content.Length;
            int chunkSize = length / parts;
            List<string> partsList = new List<string>();

            for (int i = 0; i < length; i += chunkSize)
            {
                if (i + chunkSize > length) // Adjust the last chunk size to accommodate the remainder.
                {
                    chunkSize = length - i;
                }

                string chunk = content.Substring(i, chunkSize);
                partsList.Add(chunk);
            }

            return partsList;
        }


        private static List<HttpResponseMessage> CreateHttpResponseContentMessagesForRetrieveMessage(
            Message message,
            Dictionary<string, List<string>> contentHeaders,
            Dictionary<string, List<string>> headers = null,
            int chunks = 1,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            List<HttpResponseMessage> messages = new List<HttpResponseMessage>();
            var parts = GetParts(message.StringContent, chunks);

            for (int i = 0; i < chunks; i++)
            {
                // Check if 'i' is within the range of 'parts' list
                if (i >= parts.Count)
                {
                    break;
                }

                // Create a new message for each chunk with unique content
                Message chunkMessage = new Message
                {
                    MessageId = message.MessageId,
                    StringContent = parts[i],
                };

                HttpResponseMessage httpResponseMessage = CreateHttpResponseContentMessageForRetrieveMessage(
                    chunkMessage,
                    contentHeaders,
                    headers,
                    statusCode);

                string chunkRangeValue = $"{i + 1}:{chunks}";

                if (httpResponseMessage.Content.Headers.Contains("Mex-Chunk-Range"))
                {
                    httpResponseMessage.Content.Headers.Remove("Mex-Chunk-Range");
                }

                httpResponseMessage.Content.Headers.Add("Mex-Chunk-Range", chunkRangeValue);

                messages.Add(httpResponseMessage);
            }

            return messages;
        }

        private static HttpResponseMessage CreateHttpResponseContentMessageForRetrieveMessage(
            Message message,
            Dictionary<string, List<string>> contentHeaders,
            Dictionary<string, List<string>> headers = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            string contentType = message.Headers.ContainsKey("Content-Type")
                ? message.Headers["Content-Type"].FirstOrDefault()
                : "text/plain";

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "text/plain";
            }

            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = new StringContent(message.StringContent, Encoding.UTF8, contentType)
            };

            foreach (var item in contentHeaders)
            {
                if (item.Key != "Content-Type")
                {
                    responseMessage.Content.Headers.Add(item.Key, item.Value);
                }
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    if (item.Key != "Content-Type")
                    {
                        responseMessage.Content.Headers.Add(item.Key, item.Value);
                    }
                }
            }

            return responseMessage;
        }

        private static HttpResponseMessage CreateHttpResponseContentMessageForSendMessage(
                    Message message,
                    Dictionary<string, List<string>> contentHeaders,
                    Dictionary<string, List<string>> headers = null,
                    HttpStatusCode statusCode = HttpStatusCode.OK)
        {

            string contentType = message.Headers.ContainsKey("Content-Type")
                ? message.Headers["Content-Type"].FirstOrDefault()
                : "text/plain";

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "text/plain";
            }

            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content =
                    new StringContent("{\"messageID\": \"" + message.MessageId + "\"}", Encoding.UTF8, contentType)
            };

            foreach (var item in contentHeaders)
            {
                if (item.Key != "Content-Type")
                {
                    responseMessage.Content.Headers.Add(item.Key, item.Value);
                }
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    if (item.Key != "Content-Type")
                    {
                        responseMessage.Content.Headers.Add(item.Key, item.Value);
                    }
                }
            }

            return responseMessage;
        }

        private static HttpResponseMessage CreateTrackingHttpResponseMessage(TrackMessageResponse trackMessageResponse)
        {
            string contentType = "application/json";
            string jsonContent = JsonConvert.SerializeObject(trackMessageResponse);

            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonContent, Encoding.UTF8, contentType)
            };

            return responseMessage;
        }

        private static Message GetMessageWithStringContentFromHttpResponseMessageForSend(HttpResponseMessage responseMessage)
        {
            string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;
            Dictionary<string, List<string>> contentHeaders = GetContentHeaders(responseMessage.Content.Headers);
            Dictionary<string, List<string>> headers = GetHeaders(responseMessage.Headers);

            Message message = new Message
            {
                MessageId = (JsonConvert.DeserializeObject<SendMessageResponse>(responseMessageBody)).MessageId,
                StringContent = responseMessageBody,
            };

            foreach (var item in contentHeaders)
            {
                message.Headers.Add(item.Key, item.Value);
            }

            foreach (var item in headers)
            {
                message.Headers.Add(item.Key, item.Value);
            }

            return message;
        }

        private static Message GetMessageWithStringContentFromHttpResponseMessageForReceive(HttpResponseMessage responseMessage, string messageId)
        {
            string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;
            Dictionary<string, List<string>> contentHeaders = GetContentHeaders(responseMessage.Content.Headers);
            Dictionary<string, List<string>> headers = GetHeaders(responseMessage.Headers);

            Message message = new Message
            {
                MessageId = messageId,
                StringContent = responseMessageBody,
            };

            foreach (var item in contentHeaders)
            {
                message.Headers.Add(item.Key, item.Value);
            }

            foreach (var item in headers)
            {
                message.Headers.Add(item.Key, item.Value);
            }

            return message;
        }

        private static Message GetMessageWithFileContentFromHttpResponseMessage(HttpResponseMessage responseMessage)
        {
            string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;
            Dictionary<string, List<string>> headers = GetContentHeaders(responseMessage.Content.Headers);

            Message message = new Message
            {
                MessageId = (JsonConvert.DeserializeObject<SendMessageResponse>(responseMessageBody)).MessageId,
                StringContent = responseMessageBody,
                Headers = headers
            };

            return message;
        }

        private static Message GetMessageFromTrackingHttpResponseMessage(
            string messageId,
            HttpResponseMessage responseMessage)
        {
            string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;
            Dictionary<string, List<string>> headers = GetContentHeaders(responseMessage.Content.Headers);

            Message message = new Message
            {
                MessageId = messageId,
                TrackingInfo = JsonConvert.DeserializeObject<TrackingInfo>(responseMessageBody),
                Headers = headers
            };

            return message;
        }

        private dynamic CreateRandomTrackingProperties()
        {
            return new
            {
                AddressType = GetRandomString(),
                Checksum = GetRandomString(),
                ChunkCount = GetRandomNumber(),
                CompressFlag = GetRandomString(),
                DownloadTimestamp = GetRandomString(),
                DtsId = GetRandomString(),
                EncryptedFlag = GetRandomString(),
                ExpiryTime = GetRandomString(),
                FileName = GetRandomString(),
                FileSize = GetRandomNumber(),
                IsCompressed = GetRandomString(),
                LocalId = GetRandomString(),
                MeshRecipientOdsCode = GetRandomString(),
                MessageId = GetRandomString(),
                MessageType = GetRandomString(),
                PartnerId = GetRandomString(),
                Recipient = GetRandomString(),
                RecipientName = GetRandomString(),
                RecipientOrgCode = GetRandomString(),
                RecipientSmtp = GetRandomString(),
                Sender = GetRandomString(),
                SenderName = GetRandomString(),
                SenderOdsCode = GetRandomString(),
                SenderOrgCode = GetRandomString(),
                SenderSmtp = GetRandomString(),
                Status = GetRandomString(),
                StatusSuccess = GetRandomString(),
                UploadTimestamp = GetRandomString(),
                Version = GetRandomString(),
                WorkflowId = GetRandomString()
            };
        }

        private TrackMessageResponse MapDynamicObjectToTrackMessageResponse(dynamic randomTrackingProperties)
        {
            return new TrackMessageResponse
            {
                AddressType = randomTrackingProperties.AddressType,
                Checksum = randomTrackingProperties.Checksum,
                ChunkCount = randomTrackingProperties.ChunkCount,
                CompressFlag = randomTrackingProperties.CompressFlag,
                DownloadTimestamp = randomTrackingProperties.DownloadTimestamp,
                DtsId = randomTrackingProperties.DtsId,
                EncryptedFlag = randomTrackingProperties.EncryptedFlag,
                ExpiryTime = randomTrackingProperties.ExpiryTime,
                FileName = randomTrackingProperties.FileName,
                FileSize = randomTrackingProperties.FileSize,
                IsCompressed = randomTrackingProperties.IsCompressed,
                LocalId = randomTrackingProperties.LocalId,
                MeshRecipientOdsCode = randomTrackingProperties.MeshRecipientOdsCode,
                MessageId = randomTrackingProperties.MessageId,
                MessageType = randomTrackingProperties.MessageType,
                PartnerId = randomTrackingProperties.PartnerId,
                Recipient = randomTrackingProperties.Recipient,
                RecipientName = randomTrackingProperties.RecipientName,
                RecipientOrgCode = randomTrackingProperties.RecipientOrgCode,
                RecipientSmtp = randomTrackingProperties.RecipientSmtp,
                Sender = randomTrackingProperties.Sender,
                SenderName = randomTrackingProperties.SenderName,
                SenderOdsCode = randomTrackingProperties.SenderOdsCode,
                SenderOrgCode = randomTrackingProperties.SenderOrgCode,
                SenderSmtp = randomTrackingProperties.SenderSmtp,
                Status = randomTrackingProperties.Status,
                StatusSuccess = randomTrackingProperties.StatusSuccess,
                UploadTimestamp = randomTrackingProperties.UploadTimestamp,
                Version = randomTrackingProperties.Version,
                WorkflowId = randomTrackingProperties.WorkflowId
            };
        }

        private TrackingInfo MapDynamicObjectToTrackingInfo(dynamic randomTrackingProperties)
        {
            return new TrackingInfo
            {
                AddressType = randomTrackingProperties.AddressType,
                Checksum = randomTrackingProperties.Checksum,
                ChunkCount = randomTrackingProperties.ChunkCount,
                CompressFlag = randomTrackingProperties.CompressFlag,
                DownloadTimestamp = randomTrackingProperties.DownloadTimestamp,
                DtsId = randomTrackingProperties.DtsId,
                EncryptedFlag = randomTrackingProperties.EncryptedFlag,
                ExpiryTime = randomTrackingProperties.ExpiryTime,
                FileName = randomTrackingProperties.FileName,
                FileSize = randomTrackingProperties.FileSize,
                IsCompressed = randomTrackingProperties.IsCompressed,
                LocalId = randomTrackingProperties.LocalId,
                MeshRecipientOdsCode = randomTrackingProperties.MeshRecipientOdsCode,
                MessageId = randomTrackingProperties.MessageId,
                MessageType = randomTrackingProperties.MessageType,
                PartnerId = randomTrackingProperties.PartnerId,
                Recipient = randomTrackingProperties.Recipient,
                RecipientName = randomTrackingProperties.RecipientName,
                RecipientOrgCode = randomTrackingProperties.RecipientOrgCode,
                RecipientSmtp = randomTrackingProperties.RecipientSmtp,
                Sender = randomTrackingProperties.Sender,
                SenderName = randomTrackingProperties.SenderName,
                SenderOdsCode = randomTrackingProperties.SenderOdsCode,
                SenderOrgCode = randomTrackingProperties.SenderOrgCode,
                SenderSmtp = randomTrackingProperties.SenderSmtp,
                Status = randomTrackingProperties.Status,
                StatusSuccess = randomTrackingProperties.StatusSuccess,
                UploadTimestamp = randomTrackingProperties.UploadTimestamp,
                Version = randomTrackingProperties.Version,
                WorkflowId = randomTrackingProperties.WorkflowId
            };
        }

        private static Dictionary<string, List<string>> GetHeaders(HttpResponseHeaders headers)
        {
            var dictionary = new Dictionary<string, List<string>>();

            foreach (var header in headers)
            {
                dictionary.Add(header.Key, header.Value.ToList());
            }

            return dictionary;
        }

        private static Dictionary<string, List<string>> GetContentHeaders(HttpContentHeaders headers)
        {
            var dictionary = new Dictionary<string, List<string>>();

            foreach (var header in headers)
            {
                dictionary.Add(header.Key, header.Value.ToList());
            }

            return dictionary;
        }

        private static Message CreateRandomSendMessage()
        {
            var message = CreateMessageFiller().Create();
            message.Headers.Add("Content-Type", new List<string> { "text/plain" });
            message.Headers.Add("Mex-LocalID", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Subject", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Content-Checksum", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Content-Encrypted", new List<string> { "encrypted" });
            message.Headers.Add("Mex-From", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-To", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-WorkflowID", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-FileName", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Encoding", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Chunk-Range", new List<string> { GetRandomString() });

            return message;
        }

        private static Message CreateRandomSendFileMessage()
        {
            var message = CreateMessageFiller().Create();
            message.Headers.Add("Content-Type", new List<string> { "text/plain" });
            message.Headers.Add("Mex-LocalID", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Subject", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Content-Checksum", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Content-Encrypted", new List<string> { "encrypted" });
            message.Headers.Add("Mex-From", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-To", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-WorkflowID", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-FileName", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Encoding", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Chunk-Range", new List<string> { GetRandomString() });

            return message;
        }

        private static Message CreateRandomMessage() =>
            CreateMessageFiller().Create();

        private static Filler<Message> CreateMessageFiller()
        {
            var filler = new Filler<Message>();
            filler.Setup()
                .OnProperty(message => message.Headers).Use(new Dictionary<string, List<string>>());

            return filler;
        }
    }
}
