﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace NHSISL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Fact]
        public async Task ShouldDoHandShakeAsync()
        {
            // given
            string randomToken = GetRandomString();
            bool expectedResult = true;

            this.tokenServiceMock.Setup(service =>
              service.GenerateTokenAsync())
                  .ReturnsAsync(randomToken);

            this.meshServiceMock.Setup(service =>
                service.HandshakeAsync(randomToken))
                    .ReturnsAsync(expectedResult);

            // when
            bool actualResult = await this.meshOrchestrationService.HandshakeAsync();

            // then
            actualResult.Should().BeTrue();

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.HandshakeAsync(randomToken),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
