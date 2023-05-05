// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldDoHandshakeAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            bool expectedResult = true;

            this.meshServiceMock.Setup(service =>
                service.HandshakeAsync(authorizationToken))
                    .ReturnsAsync(expectedResult);

            // when
            var actualResult = await this.meshProcessingService.HandshakeAsync(authorizationToken);

            // then
            actualResult.Should().Be(expectedResult);

            this.meshServiceMock.Verify(service =>
                service.HandshakeAsync(authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
