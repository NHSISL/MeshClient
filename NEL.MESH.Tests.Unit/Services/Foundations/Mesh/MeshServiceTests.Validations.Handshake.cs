// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnHandshakeIfTokenIsNullOrEmptyAsync(string invalidText)
        {
            // given
            string invalidAuthorizationToken = invalidText;
            Message randomMessage = CreateRandomMessage();
            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateHttpResponseMessage(inputMessage);

            var InvalidMeshArgsException =
                new InvalidMeshArgsException();

            InvalidMeshArgsException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshValidationException =
                 new MeshValidationException(
                    innerException: InvalidMeshArgsException,
                    validationSummary: GetValidationSummary(InvalidMeshArgsException.Data));

            // when
            ValueTask<bool> getMessagesTask =
                this.meshService.HandshakeAsync(invalidAuthorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    getMessagesTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.HandshakeAsync(invalidAuthorizationToken),
                    Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
