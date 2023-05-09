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
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            string authorizationToken = GetRandomString();
            bool acknowledgeResult = true;
            bool expectedResult = acknowledgeResult;

            this.meshServiceMock.Setup(service =>
                service.AcknowledgeMessageAsync(inputMessageId, authorizationToken))
                    .ReturnsAsync(acknowledgeResult);

            // when
            var actualMessage = await this.meshProcessingService
                .AcknowledgeMessageAsync(inputMessageId, authorizationToken);

            // then
            actualMessage.Should().Be(expectedResult);

            this.meshServiceMock.Verify(service =>
                service.AcknowledgeMessageAsync(inputMessageId, authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
