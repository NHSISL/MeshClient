// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.ExternalModels;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldTrackMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            dynamic randomTrackingProperties = CreateRandomTrackingProperties();
            string randomString = GetRandomString();
            string inputMessageId = randomString;
            Message randomMessage = CreateRandomMessage();
            randomMessage.MessageId = inputMessageId;

            Dictionary<string, List<string>> contentHeaders = new Dictionary<string, List<string>>
            {
                { "Content-Type", new List<string>() { "application/json; charset=utf-8" } },
                { "Content-Length", new List<string>() }
            };

            randomMessage.TrackingInfo =
                MapDynamicObjectToTrackingInfo(randomTrackingProperties);

            TrackMessageResponse trackMessageResponse =
                MapDynamicObjectToTrackMessageResponse(randomTrackingProperties);

            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateTrackingHttpResponseMessage(trackMessageResponse);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(inputMessageId, authorizationToken))
                    .ReturnsAsync(responseMessage);

            Message expectedMessage =
                GetMessageFromTrackingHttpResponseMessage(inputMessageId, responseMessage, contentHeaders);

            // when
            var actualMessage = await this.meshService.TrackMessageAsync(inputMessageId, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(inputMessageId, authorizationToken),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
