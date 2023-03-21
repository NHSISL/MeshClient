// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Moq;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Services.Mesh;
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

        private static Message CreateRandomMessage() =>
            CreateMessageFiller().Create(); 
        
        private static Filler<Message> CreateMessageFiller()
        {
            var filler = new Filler<Message>();
            filler.Setup(); return filler;
        }

        private static HttpResponseMessage CreateHttpResponseMessage(Message message)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(message.Body, Encoding.UTF8)
            };
            foreach (var item in message.Headers)
            {
                responseMessage.Content.Headers.Add(item.Key, item.Value);
            }
            return responseMessage;
        }
    }
}
