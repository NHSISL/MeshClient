// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
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
        public async Task ShouldSendMessageAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message randomMessage = CreateRandomSendMessage();
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    inputMessage.Headers["Mex-To"].First(),
                    inputMessage.Headers["Mex-WorkflowID"].First(),
                    inputMessage.StringContent,
                    inputMessage.Headers["Content-Type"].First(),
                    inputMessage.Headers["Mex-LocalID"].First(),
                    inputMessage.Headers["Mex-Subject"].First(),
                    inputMessage.Headers["Mex-Content-Encrypted"].First(),
                    authorizationToken))
                    .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageWithStringContentFromHttpResponseMessage(responseMessage);

            // when
            var actualMessage = await this.meshService.SendMessageAsync(inputMessage, authorizationToken);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    inputMessage.Headers["Mex-To"].First(),
                    inputMessage.Headers["Mex-WorkflowID"].First(),
                    inputMessage.StringContent,
                    inputMessage.Headers["Content-Type"].First(),
                    inputMessage.Headers["Mex-LocalID"].First(),
                    inputMessage.Headers["Mex-Subject"].First(),
                    inputMessage.Headers["Mex-Content-Encrypted"].First(),
                    authorizationToken),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
