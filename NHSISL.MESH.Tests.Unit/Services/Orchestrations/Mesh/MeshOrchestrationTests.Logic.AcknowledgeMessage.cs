// ---------------------------------------------------------------
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
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            string randomToken = GetRandomString();

            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;

            bool outputValidationResult = true;
            bool expectedValidationResult = outputValidationResult;

            this.tokenServiceMock.Setup(service =>
              service.GenerateTokenAsync())
                  .ReturnsAsync(randomToken);

            this.meshServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(inputMessageId, randomToken))
                    .ReturnsAsync(expectedValidationResult);

            // when
            bool actualResult =
                await this.meshOrchestrationService.AcknowledgeMessageAsync(inputMessageId);

            // then
            actualResult.Should().Be(expectedValidationResult);

            this.tokenServiceMock.Verify(service =>
               service.GenerateTokenAsync(),
                   Times.Once);

            this.meshServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(inputMessageId, randomToken),
                    Times.Once);

            this.chunkServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
