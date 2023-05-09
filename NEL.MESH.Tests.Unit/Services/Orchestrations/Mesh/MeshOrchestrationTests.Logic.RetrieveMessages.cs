// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetrieveMessagesAsync()
        {
            // given
            string randomToken = GetRandomString();

            List<string> outputMessages = GetRandomMessages();
            List<string> expectedMessages = outputMessages;

            this.tokenProcessingServiceMock.Setup(service =>
               service.GenerateTokenAsync())
                   .ReturnsAsync(randomToken);

            this.meshProcessingServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(randomToken))
                    .ReturnsAsync(outputMessages);

            // when
            List<string> actualResult =
                await this.meshOrchestrationService.RetrieveMessagesAsync();

            // then
            actualResult.Should().BeEquivalentTo(expectedMessages);

            this.tokenProcessingServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.meshProcessingServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(randomToken),
                    Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.tokenProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
