// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
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
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfMessageIsNullAndLogItAsync()
        {
            // given
            Message nullMessage = null;

            var nullMessageException =
                new NullMessageException();

            var expectedMeshValidationException =
                new MeshValidationException(nullMessageException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendMessageAsync(nullMessage);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfHeadersDictionaryIsNullAndLogItAsync()
        {
            // given

            Message messageWithNullHeaders = new Message
            {
                Headers = null
            };

            var nullHeadersException =
                new NullHeadersException();

            var expectedMeshValidationException =
                new MeshValidationException(nullHeadersException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendMessageAsync(messageWithNullHeaders);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfRequiredMessageItemsAreNullAndLogItAsync(string invalidInput)
        {
            // given
            Message randomMessage = new Message
            {
                MessageId = GetRandomString(),
                From = GetRandomString(),
                To = GetRandomString(),
                WorkflowId = GetRandomString(),
                Headers = new Dictionary<string, List<string>>(),
                Body = GetRandomString()
            };

            randomMessage.Headers.Add("Content-Type", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-FileName", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-From", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-To", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-WorkflowID", new List<string> { invalidInput });

            var invalidMeshException =
                new InvalidMeshException();

            invalidMeshException.AddData(
                 key: nameof(Message.From),
                 values: "Header value is required");

            invalidMeshException.AddData(
                 key: nameof(Message.From),
                 values: "Header value is required");

            invalidMeshException.AddData(
                 key: nameof(Message.From),
                 values: "Header value is required");

            invalidMeshException.AddData(
                 key: nameof(Message.From),
                 values: "Header value is required");

            invalidMeshException.AddData(
                 key: nameof(Message.From),
                 values: "Header value is required");

            var expectedMeshValidationException =
                new MeshValidationException(invalidMeshException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendMessageAsync(randomMessage);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfRequiredHeadersAreInvalidAndLogItAsync(string invalidInput)
        {
            // given

            Message randomMessage = new Message
            {
                MessageId = invalidInput,
                From = invalidInput,
                To = invalidInput,
                WorkflowId = invalidInput,
                Headers = new Dictionary<string, List<string>>(),
                Body = invalidInput
            };

            var invalidMeshException =
                new InvalidMeshException();

            invalidMeshException.AddData(
                 key: nameof(Message.From),
                 values: "Text is required");

            invalidMeshException.AddData(
                 key: nameof(Message.To),
                 values: "Text is required");

            invalidMeshException.AddData(
                 key: nameof(Message.WorkflowId),
                 values: "Text is required");

            invalidMeshException.AddData(
                 key: nameof(Message.Headers),
                 values: "Values is required");

            invalidMeshException.AddData(
                 key: nameof(Message.Body),
                 values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(invalidMeshException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendMessageAsync(randomMessage);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
