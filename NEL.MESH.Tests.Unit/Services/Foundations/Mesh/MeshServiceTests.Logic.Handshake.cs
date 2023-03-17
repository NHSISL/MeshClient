// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldDoHandshakeAsync()
        {
            // given
            var expectedResult = true;

            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };

            this.meshBrokerMock.Setup(broker =>
                broker.HandshakeAsync())
                    .ReturnsAsync(response);

            // when
            var actualResult = this.meshService.HandshakeAsync();

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
