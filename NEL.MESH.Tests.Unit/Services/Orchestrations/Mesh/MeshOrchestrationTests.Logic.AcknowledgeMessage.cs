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
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            string randomToken = GetRandomString();

            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;

            bool outputValidationResult = true;
            bool expectedValidationResult = outputValidationResult;

            this.tokenProcessingServiceMock.Setup(service =>
              service.GenerateTokenAsync())
                  .ReturnsAsync(randomToken);

            this.meshProcessingServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(inputMessageId, randomToken))
                    .ReturnsAsync(expectedValidationResult);

            // when
            bool actualResult =
                await this.meshOrchestrationService.AcknowledgeMessageAsync(inputMessageId);

            // then
            actualResult.Should().Be(expectedValidationResult);

            this.tokenProcessingServiceMock.Verify(service =>
               service.GenerateTokenAsync(),
                   Times.Once);

            this.meshProcessingServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(inputMessageId, randomToken),
                    Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.tokenProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
