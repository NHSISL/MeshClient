// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowValidationExceptionOnSendFileIfMessageIsNullAsync()
        {
            // given
            Message nullMessage = null;
            string authorizationToken = GetRandomString();

            var nullMessageException =
                new NullMessageException();

            var expectedMeshValidationException =
                new MeshValidationException(nullMessageException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendFileAsync(nullMessage, authorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendFileIfHeadersDictionaryIsNullAsync()
        {
            // given
            string authorizationToken = GetRandomString();

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
                this.meshService.SendFileAsync(messageWithNullHeaders, authorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSendFileIfRequiredMessageItemsAreNullAsync(
            string invalidInput)
        {
            // given
            string invalidAuthorizationToken = invalidInput;
            byte[] invalidContent = null;

            Message randomMessage = new Message
            {
                MessageId = GetRandomString(),
                Headers = new Dictionary<string, List<string>>(),
                FileContent = invalidContent
            };

            randomMessage.Headers.Add("Mex-From", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-To", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-WorkflowID", new List<string> { invalidInput });
            randomMessage.Headers.Add("Content-Type", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-Subject", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-FileName", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-Content-Checksum", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-Content-Encrypted", new List<string> { invalidInput });

            var invalidMeshException =
                new InvalidMeshException();

            invalidMeshException.AddData(
                key: nameof(Message.FileContent),
                values: "Content is required");

            invalidMeshException.AddData(
                key: "Content-Type",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: "Mex-FileName",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: "Mex-From",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: "Mex-To",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: "Mex-WorkflowID",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: "Mex-Content-Checksum",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: "Mex-Content-Encrypted",
                values: "Header value is required");

            invalidMeshException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(innerException: invalidMeshException);

            // when
            ValueTask<Message> addMessageTask =
                this.meshService.SendFileAsync(randomMessage, invalidAuthorizationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
