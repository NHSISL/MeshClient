// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveMessagesAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            List<string> randomMessages = GetRandomMessages();
            List<string> expectedMessages = randomMessages;

            HttpResponseMessage responseMessage =
                CreateGetMessagesHttpResponseMessage(randomMessages);

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessagesAsync(authorizationToken, default))
                    .ReturnsAsync(responseMessage);

            // when
            List<string> actualMessages =
                await this.meshService.RetrieveMessagesAsync(authorizationToken);

            // then
            actualMessages.Should().BeEquivalentTo(expectedMessages);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessagesAsync(authorizationToken, default),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
