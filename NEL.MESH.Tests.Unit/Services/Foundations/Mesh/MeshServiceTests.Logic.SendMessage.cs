// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
            Message randomMessage = CreateRandomMessage();
            randomMessage.Headers["Content-Type"] = new List<string> { "text/plain" };
            randomMessage.Headers["Mex-LocalID"] = new List<string> { GetRandomString() };
            randomMessage.Headers["Mex-Subject"] = new List<string> { GetRandomString() };
            randomMessage.Headers["Mex-Content-Encrypted"] = new List<string> { "encrypted" };
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    inputMessage.To,
                    inputMessage.WorkflowId,
                    inputMessage.Body,
                    inputMessage.Headers["Content-Type"].First(),
                    inputMessage.Headers["Mex-LocalID"].First(),
                    inputMessage.Headers["Mex-Subject"].First(),
                    inputMessage.Headers["Mex-Content-Encrypted"].First()
                    ))
                    .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageFromHttpResponseMessage(responseMessage);

            // when
            var actualMessage = await this.meshService.SendMessageAsync(inputMessage);

            // then
            actualMessage.Should().Be(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.HandshakeAsync(),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
