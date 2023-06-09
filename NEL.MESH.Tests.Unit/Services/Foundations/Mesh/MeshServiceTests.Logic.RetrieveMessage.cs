﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSinglePartMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;

            Dictionary<string, List<string>> contentHeaders = new Dictionary<string, List<string>>
            {
                { "content-type", new List<string>{ "text/plain" } },
                { "content-length", new List<string>() }
            };

            Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>
            {
                { "content-encoding", new List<string>() },
                { "mex-filename", new List<string>() },
                { "mex-from", new List<string>() },
                { "mex-to", new List<string>() },
                { "mex-workflowid", new List<string>() },
                { "mex-chunk-range", new List<string>{"{1:1}"} },
                { "mex-localid", new List<string>() },
                { "mex-subject", new List<string>() },
                { "mex-content-checksum", new List<string>() },
                { "Mex-Content-Encrypted", new List<string>() },
                { "mex-clientversion", new List<string>() },
                { "mex-osversion", new List<string>() },
                { "mex-osarchitecture", new List<string>() },
                { "mex-javaversion", new List<string>() }
            };

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessageForRetrieveMessage(
                inputMessage,
                contentHeaders,
                headers,
                HttpStatusCode.OK);

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessageAsync(inputMessage.MessageId, authorizationToken))
                    .ReturnsAsync(responseMessage);

            Message expectedMessage =
                GetMessageFromHttpResponseMessageForReceive(responseMessage, inputMessage.MessageId);

            // when
            var actualMessage = await this.meshService
                .RetrieveMessageAsync(inputMessage.MessageId, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(inputMessage.MessageId, authorizationToken),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveMultiPartMessagesAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message randomMessage = CreateRandomSendMessage();
            int chunks = GetRandomNumber();
            Message inputMessage = randomMessage;

            if (chunks > randomMessage.FileContent.Length)
            {
                chunks = randomMessage.FileContent.Length;
            }

            Dictionary<string, List<string>> contentHeaders = new Dictionary<string, List<string>>
            {
                { "content-type", new List<string>{ "text/plain" } },
                { "content-length", new List<string>() },
                { "Last-Modified", new List<string>() },
            };

            Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>
            {
                { "content-encoding", new List<string>() },
                { "mex-filename", new List<string>() },
                { "mex-from", new List<string>() },
                { "mex-to", new List<string>() },
                { "mex-workflowid", new List<string>() },
                { "mex-localid", new List<string>() },
                { "mex-subject", new List<string>() },
                { "mex-content-checksum", new List<string>() },
                { "Mex-Content-Encrypted", new List<string>() },
                { "mex-clientversion", new List<string>() },
                { "mex-osversion", new List<string>() },
                { "mex-osarchitecture", new List<string>() },
                { "mex-javaversion", new List<string>() }
            };

            List<HttpResponseMessage> responseMessages =
                CreateHttpResponseContentMessagesForRetrieveMessage(
                    inputMessage,
                    contentHeaders,
                    headers,
                    chunks,
                    HttpStatusCode.PartialContent);

            Message expectedMessage = new Message();

            for (int i = 0; i < chunks; i++)
            {
                if (i == 0)
                {
                    this.meshBrokerMock.Setup(broker =>
                        broker.GetMessageAsync(inputMessage.MessageId, authorizationToken))
                            .ReturnsAsync(responseMessages[i]);

                    expectedMessage = GetMessageFromHttpResponseMessageForReceive(
                        responseMessages[i], inputMessage.MessageId);
                }
                else
                {
                    this.meshBrokerMock.Setup(broker =>
                        broker.GetMessageAsync(
                            inputMessage.MessageId,
                            It.Is(SameStringAs((i + 1).ToString())),
                            authorizationToken))
                                .ReturnsAsync(responseMessages[i]);
                }
            }

            expectedMessage.FileContent = responseMessages
                .SelectMany(message =>
                    GetMessageFromHttpResponseMessageForReceive(message, inputMessage.MessageId)
                        .FileContent).ToArray();

            // when
            var actualMessage = await this.meshService
                .RetrieveMessageAsync(inputMessage.MessageId, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            for (int i = 0; i < chunks; i++)
            {
                if (i == 0)
                {
                    this.meshBrokerMock.Verify(broker =>
                        broker.GetMessageAsync(inputMessage.MessageId, authorizationToken),
                            Times.Once);
                }
                else
                {
                    this.meshBrokerMock.Verify(broker =>
                        broker.GetMessageAsync(inputMessage.MessageId, It.Is(SameStringAs((i + 1).ToString())), authorizationToken),
                            Times.Once);
                }
            }

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
