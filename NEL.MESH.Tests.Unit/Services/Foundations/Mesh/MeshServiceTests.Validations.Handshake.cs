// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
                { "Content-Encoding", new List<string>() },
                { "Mex-FileName", new List<string>() },
                { "Mex-From", new List<string>() },
                { "Mex-To", new List<string>() },
                { "Mex-WorkflowID", new List<string>() },
                { "Mex-Chunk-Range", new List<string>() },
                { "Mex-LocalID", new List<string>() },
                { "Mex-Subject", new List<string>() },
                { "Mex-Content-Checksum", new List<string>() },
                { "Mex-Content-Encrypted", new List<string>() },
                { "Mex-ClientVersion", new List<string>() },
                { "Mex-OSVersion", new List<string>() },
                { "Mex-OSArchitecture", new List<string>() },
                { "Mex-JavaVersion", new List<string>() }
            };

            HttpResponseMessage responseMessage = CreateHttpResponseContentMessage(inputMessage, contentHeaders);

            var InvalidMeshArgsException =
                new InvalidArgumentsMeshException();

            InvalidMeshArgsException.AddData(
                key: "Token",
                values: "Text is required");

            var expectedMeshValidationException =
                 new MeshValidationException(innerException: InvalidMeshArgsException);

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
