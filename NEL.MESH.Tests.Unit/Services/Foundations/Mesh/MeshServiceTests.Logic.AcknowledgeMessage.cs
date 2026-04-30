// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net;
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
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            string randomMessageId = GetRandomString();

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            bool expectedResult = response.IsSuccessStatusCode;

            this.meshBrokerMock.Setup(broker =>
                broker.AcknowledgeMessageAsync(randomMessageId, authorizationToken))
                    .ReturnsAsync(response);

            // when
            bool actualResult = await this.meshService.AcknowledgeMessageAsync(randomMessageId, authorizationToken);

            // then
            actualResult.Should().Be(expectedResult);

            this.meshBrokerMock.Verify(broker =>
                broker.AcknowledgeMessageAsync(randomMessageId, authorizationToken),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
