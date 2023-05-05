// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Processings.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnHandshakeIfTokenIsNullOrEmptyAsync(string invalidText)
        {
            // given
            string invalidAuthorizationToken = invalidText;

            var invalidArgumentsMeshProcessingException =
                new InvalidArgumentsMeshProcessingException();

            invalidArgumentsMeshProcessingException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshProcessingValidationException =
                 new MeshProcessingValidationException(innerException: invalidArgumentsMeshProcessingException);

            // when
            ValueTask<bool> getMessagesTask =
               this.meshProcessingService.HandshakeAsync(invalidAuthorizationToken);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(() =>
                    getMessagesTask.AsTask());

            // then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.Verify(broker =>
                broker.HandshakeAsync(invalidAuthorizationToken),
                    Times.Never);

            this.meshServiceMock.VerifyNoOtherCalls();
            // then
        }
    }
}
