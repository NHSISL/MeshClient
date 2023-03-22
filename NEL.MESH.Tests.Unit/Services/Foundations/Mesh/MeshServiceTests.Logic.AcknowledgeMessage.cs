// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldDoAcknowledgeMessageAsync()
        {
            // given
            Message someMessage = CreateRandomSendMessage();

            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };

            bool expectedResult = response.IsSuccessStatusCode;

            this.meshBrokerMock.Setup(broker =>
                broker.AcknowledgeMessageAsync(someMessage.MessageId))
                    .ReturnsAsync(response);

            // when
            var actualResult = await this.meshService.AcknowledgeMessageAsync(someMessage.MessageId);

            // then
            actualResult.Should().Be(expectedResult);

            this.meshBrokerMock.Verify(broker =>
                broker.AcknowledgeMessageAsync(someMessage.MessageId),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
