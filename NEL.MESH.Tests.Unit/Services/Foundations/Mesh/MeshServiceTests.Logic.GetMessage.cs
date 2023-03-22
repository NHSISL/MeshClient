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
            Message randomMessage = CreateRandomMessage();
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessageAsync(inputMessage.MessageId))
                    .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageFromHttpResponseMessage(responseMessage);

            // when
            var actualMessage = await this.meshService.RetieveMessageAsync(inputMessage.MessageId);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessageAsync(inputMessage.MessageId),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
