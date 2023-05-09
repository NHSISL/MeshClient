// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveMessageAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            string authorizationToken = GetRandomString();
            Message randomTrackingMessage = CreateRandomMessage();
            Message trackingOutputMessage = randomTrackingMessage;
            Message expectedMessage = trackingOutputMessage.DeepClone();

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageAsync(inputMessageId, authorizationToken))
                    .ReturnsAsync(trackingOutputMessage);

            // when
            var actualMessage = await this.meshProcessingService
                .RetrieveMessageAsync(inputMessageId, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(inputMessageId, authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
