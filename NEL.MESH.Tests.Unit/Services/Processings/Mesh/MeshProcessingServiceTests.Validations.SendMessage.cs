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
        public async Task ShouldThrowValidationExceptionOnSendMessageIfTokenIsNullOrEmptyAsync(string invalidText)
        {
            // given
            string invalidAuthorizationToken = invalidText;
            Message randomSendMessage = CreateRandomMessage();
            Message inputMessage = randomSendMessage;

            var invalidArgumentsMeshProcessingException =
                new InvalidArgumentsMeshProcessingException();

            invalidArgumentsMeshProcessingException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshProcessingValidationException =
                new MeshProcessingValidationException(innerException: invalidArgumentsMeshProcessingException);

            // when
            ValueTask<Message> getMessagesTask =
                this.meshProcessingService.SendMessageAsync(inputMessage, invalidAuthorizationToken);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(() =>
                    getMessagesTask.AsTask());

            // then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfMessageIsNullAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message nullSendMessage = null;

            var invalidArgumentsMeshProcessingException =
                new InvalidArgumentsMeshProcessingException();

            invalidArgumentsMeshProcessingException.AddData(
                key: nameof(Message),
                values: "Message is required");

            var expectedMeshProcessingValidationException =
                new MeshProcessingValidationException(innerException: invalidArgumentsMeshProcessingException);

            // when
            ValueTask<Message> getMessagesTask =
                this.meshProcessingService.SendMessageAsync(nullSendMessage, authorizationToken);

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