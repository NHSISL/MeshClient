// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveMessageIfTokenIsNullOrEmptyAsync(string invalidText)
        {
            // given
            string invalidMessageId = invalidText;
            string invalidAuthorizationToken = invalidText;

            var invalidArgumentsMeshProcessingException =
                new InvalidArgumentsMeshProcessingException();

            invalidArgumentsMeshProcessingException.AddData(
                key: nameof(Message.MessageId),
                values: "Text is required");

            invalidArgumentsMeshProcessingException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshProcessingValidationException =
                 new MeshProcessingValidationException(innerException: invalidArgumentsMeshProcessingException);

            // when
            ValueTask<Message> getMessagesTask =
                this.meshProcessingService.RetrieveMessageAsync(invalidMessageId, invalidAuthorizationToken);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(() =>
                    getMessagesTask.AsTask());

            // then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageAsync(invalidMessageId, invalidAuthorizationToken),
                    Times.Never);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}