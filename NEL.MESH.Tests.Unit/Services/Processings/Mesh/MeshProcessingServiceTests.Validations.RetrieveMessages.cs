﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveMessagesIfTokenIsNullOrEmptyAsync(string invalidText)
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
            ValueTask<List<string>> getMessagesTask =
               this.meshProcessingService.RetrieveMessagesAsync(invalidAuthorizationToken);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(() =>
                    getMessagesTask.AsTask());

            // then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.Verify(broker =>
                broker.RetrieveMessagesAsync(invalidAuthorizationToken),
                    Times.Never);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}