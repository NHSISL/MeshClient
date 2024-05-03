// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace NHSISL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldDoHandshakeAsync()
        {
            // given
            string authorizationToken = GetRandomString();

            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };

            bool expectedResult = response.IsSuccessStatusCode;

            this.meshBrokerMock.Setup(broker =>
                broker.HandshakeAsync(authorizationToken))
                    .ReturnsAsync(response);

            // when
            var actualResult = await this.meshService.HandshakeAsync(authorizationToken);

            // then
            actualResult.Should().Be(expectedResult);

            this.meshBrokerMock.Verify(broker =>
                broker.HandshakeAsync(authorizationToken),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
