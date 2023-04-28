// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh.ExternalModels;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnTrackMessageIfRequiredMessageArgumenstAreinvalidAsync(
            string invalidInput)
        {
            // given
            string invalidAuthorizationToken = invalidInput;
            dynamic randomTrackingProperties = CreateRandomTrackingProperties();
            string randomString = GetRandomString();
            string inputMessageId = invalidInput;
            Message randomMessage = CreateRandomMessage();
            randomMessage.MessageId = inputMessageId;

            Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>
            {
                { "Content-Encoding", new List<string>() },
                { "Mex-FileName", new List<string>() },
                { "Mex-From", new List<string>() },
                { "Mex-To", new List<string>() },
                { "Mex-WorkflowID", new List<string>() },
                { "Mex-Chunk-Range", new List<string>() },
                { "Mex-LocalID", new List<string>() },
                { "Mex-Subject", new List<string>() },
                { "Mex-Content-Checksum", new List<string>() },
                { "Mex-Content-Encrypted", new List<string>() },
                { "Mex-ClientVersion", new List<string>() },
                { "Mex-OSVersion", new List<string>() },
                { "Mex-OSArchitecture", new List<string>() },
                { "Mex-JavaVersion", new List<string>() }
            };

            randomMessage.TrackingInfo =
                MapDynamicObjectToTrackingInfo(randomTrackingProperties);

            TrackMessageResponse trackMessageResponse =
                MapDynamicObjectToTrackMessageResponse(randomTrackingProperties);

            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateTrackingHttpResponseMessage(trackMessageResponse);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(inputMessageId, invalidAuthorizationToken))
            .ReturnsAsync(responseMessage);

            Message expectedMessage =
                GetMessageFromTrackingHttpResponseMessage(inputMessageId, responseMessage, headers);

            var invalidMeshArgsException =
                new InvalidMeshArgsException();

            invalidMeshArgsException.AddData(
                key: nameof(Message.MessageId),
                values: "Text is required");

            invalidMeshArgsException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                   innerException: invalidMeshArgsException,
                   validationSummary: GetValidationSummary(invalidMeshArgsException.Data));

            // when
            ValueTask<Message> trackMessageTask =
                this.meshService.TrackMessageAsync(inputMessageId, invalidAuthorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    trackMessageTask.AsTask());

            // then
            actualMeshValidationException.Should().BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(inputMessageId, invalidAuthorizationToken),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
