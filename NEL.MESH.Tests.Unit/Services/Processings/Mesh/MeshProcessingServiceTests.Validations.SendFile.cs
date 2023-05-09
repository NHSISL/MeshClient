// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowValidationExceptionOnSendFileIfTokenIsNullOrEmptyAsync(string invalidText)
        {
            // given
            string invalidAuthorizationToken = invalidText;
            Message randomSendFile = CreateRandomMessage();
            Message inputMessage = randomSendFile;

            var invalidArgumentsMeshProcessingException =
                new InvalidArgumentsMeshProcessingException();

            invalidArgumentsMeshProcessingException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshProcessingValidationException =
                new MeshProcessingValidationException(innerException: invalidArgumentsMeshProcessingException);

            // when
            ValueTask<Message> getMessagesTask =
                this.meshProcessingService.SendFileAsync(inputMessage, invalidAuthorizationToken);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(() =>
                    getMessagesTask.AsTask());

            // then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendFileIfMessageIsNullAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message nullSendFile = null;

            var invalidArgumentsMeshProcessingException =
                new InvalidArgumentsMeshProcessingException();

            invalidArgumentsMeshProcessingException.AddData(
                key: nameof(Message),
                values: "Message is required");

            var expectedMeshProcessingValidationException =
                 new MeshProcessingValidationException(innerException: invalidArgumentsMeshProcessingException);

            // when
            ValueTask<Message> getMessagesTask =
                this.meshProcessingService.SendFileAsync(nullSendFile, authorizationToken);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(() =>
                    getMessagesTask.AsTask());

            // then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}