// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnretrieveMessagesIfMessageIdIsNullOrEmptyAsync(
            string invalidText)
        {
            // given
            string invalidAuthorizationToken = invalidText;
            Message randomMessage = CreateRandomMessage();
            Message inputMessage = randomMessage;

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessage(
                inputMessage,
                new Dictionary<string, List<string>>(),
                new Dictionary<string, List<string>>());

            this.meshBrokerMock.Setup(broker =>
              broker.GetMessagesAsync(inputMessage.MessageId))
                  .ReturnsAsync(responseMessage);

            var InvalidMeshArgsException =
                new InvalidArgumentsMeshException();

            InvalidMeshArgsException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshValidationException =
                 new MeshValidationException(innerException: InvalidMeshArgsException);

            // when
            ValueTask<List<string>> getMessagesTask =
                this.meshService.RetrieveMessagesAsync(invalidAuthorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    getMessagesTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessagesAsync(invalidAuthorizationToken),
                    Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
