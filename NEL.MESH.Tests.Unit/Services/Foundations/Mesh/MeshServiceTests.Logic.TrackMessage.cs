// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldTrackMessageAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputMessageId = randomString;

            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(inputMessageId))
                    .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageFromHttpResponseMessage(responseMessage);

            // when
            var actualMessage = await this.meshService.TrackMessageAsync(inputMessageId);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(inputMessageId),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
