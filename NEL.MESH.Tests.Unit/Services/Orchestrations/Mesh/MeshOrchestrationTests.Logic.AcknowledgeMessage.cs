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

            bool outputValidationResult = true;
            bool expectedValidationResult = outputValidationResult;

            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;

            this.meshServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(inputMessageId, randomToken))
                    .ReturnsAsync(expectedValidationResult);

            // when
            bool actualResult = await this.meshOrchestrationService.AcknowledgeMessageAsync(inputMessageId);

            // then
            actualResult.Should().BeTrue();

            this.meshServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(inputMessageId, randomToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
