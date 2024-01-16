// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldThrowValidationExceptionOnHandshakeIfTokenIsNullOrEmptyAsync(string invalidText)
        {
            // given
            string invalidAuthorizationToken = invalidText;
            Message randomMessage = CreateRandomMessage();
            Message inputMessage = randomMessage;

            Dictionary<string, List<string>> contentHeaders = new Dictionary<string, List<string>>
            {
                { "content-encoding", new List<string>() },
                { "mex-filename", new List<string>() },
                { "mex-from", new List<string>() },
                { "mex-to", new List<string>() },
                { "mex-workflowid", new List<string>() },
                { "mex-chunk-range", new List<string>() },
                { "mex-localid", new List<string>() },
                { "mex-subject", new List<string>() },
                { "mex-content-checksum", new List<string>() },
                { "Mex-Content-Encrypted", new List<string>() },
                { "mex-clientversion", new List<string>() },
                { "mex-osversion", new List<string>() },
                { "mex-osarchitecture", new List<string>() },
                { "mex-javaversion", new List<string>() }
            };

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessageForSendMessage(inputMessage, contentHeaders);

            var invalidMeshArgsException = new InvalidArgumentsMeshException(
                    message: "Invalid MESH argument valiation errors occurred, " +
                    "please correct the errors and try again.");

            invalidMeshArgsException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshValidationException =
                 new MeshValidationException(
                     message: "Message validation errors occurred, please try again.",
                     innerException: invalidMeshArgsException);

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
