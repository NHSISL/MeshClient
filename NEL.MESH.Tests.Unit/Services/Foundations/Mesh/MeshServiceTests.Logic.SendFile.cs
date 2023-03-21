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
        public async Task ShouldSendFileAsync()
        {
            // given
            Message randomMessage = CreateRandomMessage();
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            this.meshBrokerMock.Setup(broker =>
                broker.SendFileAsync(
                    mailboxTo: inputMessage.To,
                    workflowId: inputMessage.WorkflowId,
                    message: inputMessage.Body,
                    contentType: inputMessage.,
                    localId: inputMessage.loc,
                    subject: xxx,
                    contentEncrypted: xxx));
                        .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageFromHttpResponseMessage(responseMessage);

            // when
            var actualMessage = await this.meshService.SendMessageAsync(inputMessage);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker => 
                broker.HandshakeAsync(),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
