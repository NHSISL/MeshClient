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
        public async Task ShouldDoRetrieveMessagesAsync()
        {
            // given
            string randomToken = GetRandomString();

            List<string> outputMessages = GetRandomMessages(GetRandomNumber());
            List<string> expectedMessages = outputMessages;

            this.tokenServiceMock.Setup(service =>
               service.GenerateTokenAsync())
                   .ReturnsAsync(randomToken);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(randomToken))
                    .ReturnsAsync(expectedMessages);

            // when
            List<string> actualResult = await this.meshOrchestrationService.RetrieveMessagesAsync();

            // then
            actualResult.Should().BeSameAs(expectedMessages);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(randomToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
