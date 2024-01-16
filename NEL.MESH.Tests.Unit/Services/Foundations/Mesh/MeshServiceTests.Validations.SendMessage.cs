// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfMessageIsNullAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            Message nullMessage = null;

            var nullMessageException =
                new NullMessageException(message: "Message headers dictionary is null.");

            var expectedMeshValidationException =
                new MeshValidationException(
                    message: "Message validation errors occurred, please try again.",
                    innerException: nullMessageException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendMessageAsync(nullMessage, authorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfHeadersDictionaryIsNullAsync()
        {
            // given
            string authorizationToken = GetRandomString();

            Message messageWithNullHeaders = new Message
            {
                Headers = null
            };

            var nullHeadersException =
                new NullHeadersException(message: "Message headers dictionary is null.");

            var expectedMeshValidationException =
                new MeshValidationException(
                    message: "Message validation errors occurred, please try again.",
                    innerException: nullHeadersException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendMessageAsync(messageWithNullHeaders, authorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfRequiredMessageItemsAreNullAsync(
            string invalidInput)
        {
            // given
            string invalidAuthorizationToken = invalidInput;

            Message randomMessage = new Message
            {
                MessageId = GetRandomString(),
                Headers = new Dictionary<string, List<string>>(),
                FileContent = null
            };

            randomMessage.Headers.Add("mex-from", new List<string> { invalidInput });
            randomMessage.Headers.Add("mex-to", new List<string> { invalidInput });
            randomMessage.Headers.Add("mex-workflowid", new List<string> { invalidInput });

            var invalidMeshException =
                new InvalidMeshException(message: "Invalid message, please correct errors and try again.");

            invalidMeshException.AddData(
                key: "Token",
                values: "Text is required");

            invalidMeshException.AddData(
                key: "mex-from",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: "mex-to",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: "mex-workflowid",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: nameof(Message.FileContent),
                values: "Content is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                    message: "Message validation errors occurred, please try again.",
                    innerException: invalidMeshException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendMessageAsync(randomMessage, invalidAuthorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfLengthRequirementsNotMetAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            string chunkSize = "{1:2}";
            Message randomFileMessage = CreateRandomInvalidSendMessage(chunkSize);

            Message inputFileMessage = randomFileMessage;

            var invalidMeshException =
               new InvalidMeshException(message: "Invalid message, please correct errors and try again.");

            invalidMeshException.AddData(
                key: "mex-from",
                values: $"Text length should not be greater than 100");

            invalidMeshException.AddData(
                key: "mex-to",
                values: $"Text length should not be greater than 100");

            invalidMeshException.AddData(
                key: "mex-workflowid",
                values: $"Text length should not be greater than 300");

            invalidMeshException.AddData(
                key: "mex-chunk-range",
                values: $"Text length should not be greater than 20");

            invalidMeshException.AddData(
                key: "mex-subject",
                values: $"Text length should not be greater than 500");

            invalidMeshException.AddData(
                key: "mex-localid",
                values: $"Text length should not be greater than 300");

            invalidMeshException.AddData(
                key: "mex-filename",
                values: $"Text length should not be greater than 300");

            invalidMeshException.AddData(
                key: "mex-content-checksum",
                values: $"Text length should not be greater than 100");

            var expectedMeshValidationException =
                new MeshValidationException(
                    message: "Message validation errors occurred, please try again.",
                    innerException: invalidMeshException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendMessageAsync(inputFileMessage, authorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfMessageIdAndChunkSizeIsNullAsync(
            string invalidInput)
        {
            // given
            string invalidAuthorizationToken = GetRandomString();
            string chunkSize = "{2:2}";
            Message randomMessage = CreateRandomSendMessage(chunkSize);

            randomMessage.MessageId = invalidInput;

            var invalidMeshException =
               new InvalidMeshException(message: "Invalid message, please correct errors and try again.");

            invalidMeshException.AddData(
                key: nameof(Message.MessageId),
                values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                    message: "Message validation errors occurred, please try again.",
                    innerException: invalidMeshException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendMessageAsync(randomMessage, invalidAuthorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
