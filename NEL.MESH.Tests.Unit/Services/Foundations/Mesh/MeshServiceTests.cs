﻿// ---------------------------------------------------------------
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
using NEL.MESH.Models.Foundations.Mesh.ExternalModeld;
using NEL.MESH.Services.Mesh;
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

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

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

        private static HttpResponseMessage CreateHttpResponseMessage(Message message)
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
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(message.Body, Encoding.UTF8, contentType)
            };

            responseMessage.Content.Headers.Add("Content-Type", string.Empty);
            responseMessage.Content.Headers.Add("Content-Encoding", string.Empty);
            responseMessage.Content.Headers.Add("Mex-FileName", string.Empty);
            responseMessage.Content.Headers.Add("Mex-From", string.Empty);
            responseMessage.Content.Headers.Add("Mex-To", string.Empty);
            responseMessage.Content.Headers.Add("Mex-WorkflowID", string.Empty);
            responseMessage.Content.Headers.Add("Mex-Chunk-Range", string.Empty);
            responseMessage.Content.Headers.Add("Mex-LocalID", string.Empty);
            responseMessage.Content.Headers.Add("Mex-Subject", string.Empty);
            responseMessage.Content.Headers.Add("Mex-Content-Checksum", string.Empty);
            responseMessage.Content.Headers.Add("Mex-Content-Encrypted", string.Empty);
            responseMessage.Content.Headers.Add("Authorization", string.Empty);
            responseMessage.Content.Headers.Add("Mex-ClientVersion", string.Empty);
            responseMessage.Content.Headers.Add("Mex-OSVersion", string.Empty);
            responseMessage.Content.Headers.Add("Mex-OSArchitecture", string.Empty);
            responseMessage.Content.Headers.Add("Mex-JavaVersion", string.Empty);
            responseMessage.Content.Headers.Add("Mex-Chunk-Range", string.Empty);
            responseMessage.Content.Headers.Add("Mex-Chunk-Range", string.Empty);
            responseMessage.Content.Headers.Add("Mex-Chunk-Range", string.Empty);
            responseMessage.Content.Headers.Add("Mex-Chunk-Range", string.Empty);

            foreach (var item in message.Headers)
            {
                if (message.Headers.ContainsKey(item.Key))
                {
                    responseMessage.Content.Headers.Remove(item.Key);
                    responseMessage.Content.Headers.Add(item.Key, item.Value);
                }
                else
                {
                    responseMessage.Content.Headers.Add(item.Key, item.Value);
                }
            }

            return responseMessage;
        }

        private static Message GetMessageFromHttpResponseMessage(HttpResponseMessage responseMessage)
        {
            string responseMessageBody = responseMessage.Content.ReadAsStringAsync().Result;
            Dictionary<string, List<string>> headers = GetHeaders(responseMessage.Headers);

            Message message = new Message
            {
                MessageId = (JsonConvert.DeserializeObject<SendMessageResponse>(responseMessageBody)).MessageId,
                From = headers["Mex-From"].First(),
                To = headers["Mex-To"].First(),
                WorkflowId = headers["Mex-WorkflowID"].First(),
                Body = responseMessageBody,
            };

            return message;
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


        private static Message CreateRandomMessage() =>
            CreateMessageFiller().Create();

        private static Filler<Message> CreateMessageFiller()
        {
            var filler = new Filler<Message>();
            filler.Setup();

            return filler;
        }
    }
}
