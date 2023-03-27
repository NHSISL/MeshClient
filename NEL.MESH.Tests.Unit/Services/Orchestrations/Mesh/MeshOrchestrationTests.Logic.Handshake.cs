// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Fact]
        public async Task ShouldDoHandShakeAsync()
        {
            // given
            bool expectedResult = true;

            this.meshServiceMock.Setup(service =>
                service.HandshakeAsync())
                    .ReturnsAsync(expectedResult);

            // when
            bool actualResult = await this.meshOrchestrationService.HandshakeAsync();

            // then
            actualResult.Should().BeTrue();

            this.meshServiceMock.Verify(service =>
                service.HandshakeAsync(),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
