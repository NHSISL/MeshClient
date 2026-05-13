// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAcknowledgeMessageIfRequiredArgsAreInvalidAsync(
            string invalidInput)
        {
            // given
            string invalidMessageId = invalidInput;
            string invalidAuthorizationToken = invalidInput;

            var invalidMeshArgsException = new InvalidArgumentsMeshException(
                message: "Invalid MESH argument validation errors occurred, " +
                "please correct the errors and try again.");

            invalidMeshArgsException.AddData(
                key: nameof(Message.MessageId),
                values: "Text is required");

            invalidMeshArgsException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshValidationException = new MeshValidationException(
                message: "Message validation errors occurred, please try again.",
                innerException: invalidMeshArgsException);

            // when
            ValueTask<bool> acknowledgeMessageTask =
                this.meshService.AcknowledgeMessageAsync(invalidMessageId, invalidAuthorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(acknowledgeMessageTask.AsTask);

            // then
            actualMeshValidationException.Should().BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.AcknowledgeMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
