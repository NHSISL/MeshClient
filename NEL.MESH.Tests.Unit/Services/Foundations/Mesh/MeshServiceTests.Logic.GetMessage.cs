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
        public async Task ShouldGetMessageAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputString = randomString;

            Message randomMessage = CreateRandomMessage();
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessageAsync(inputString))
                    .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageFromHttpResponseMessage(responseMessage);

            // when
            var actualMessage = await this.meshService.GetMessageAsync(inputString);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(inputString),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
