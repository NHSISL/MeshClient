// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Clients.Token.Exceptions;
using NEL.MESH.Models.Processings.Mesh;
using NEL.MESH.Models.Processings.Token;
using NEL.MESH.Models.Processings.Tokens;
using Xeptions;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Token
{
    public partial class TokenProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnGenerateTokenIfDependencyValidationErrorOccurs(
            Xeption dependencyValidationException)
        {
            // given
            var expectedTokenProcessingDependencyValidationException =
                new TokenProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<string> GenerateTokenTask =
                this.tokenProcessingService.GenerateTokenAsync();

            TokenProcessingDependencyValidationException actualTokenProcessingDependencyValidationException =
                await Assert.ThrowsAsync<TokenProcessingDependencyValidationException>(GenerateTokenTask.AsTask);

            // then
            actualTokenProcessingDependencyValidationException.Should()
                .BeEquivalentTo(expectedTokenProcessingDependencyValidationException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGenerateTokenIfDependencyErrorOccurs(
            Xeption dependencyException)
        {
            // given
            string authorizationToken = GetRandomString();

            var expectedTokenProcessingDependencyException =
                new TokenProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<string> GenerateTokenTask =
                this.tokenProcessingService.GenerateTokenAsync();

            TokenProcessingDependencyException actualTokenProcessingDependencyException =
                await Assert.ThrowsAsync<TokenProcessingDependencyException>(GenerateTokenTask.AsTask);

            // then
            actualTokenProcessingDependencyException.Should()
                .BeEquivalentTo(expectedTokenProcessingDependencyException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.tokenServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGenerateTokenIfServiceErrorOccurs()
        {
            // given
            var serviceException = new Exception();

            var failedTokenProcessingServiceException =
                new FailedTokenProcessingServiceException(serviceException);

            var expectedTokenProcessingServiceException =
                new TokenProcessingServiceException(failedTokenProcessingServiceException);

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> GenerateTokenTask =
                this.tokenProcessingService.GenerateTokenAsync();

            TokenProcessingServiceException actualTokenProcessingServiceException =
                await Assert.ThrowsAsync<TokenProcessingServiceException>(GenerateTokenTask.AsTask);

            // then
            actualTokenProcessingServiceException.Should()
                .BeEquivalentTo(expectedTokenProcessingServiceException);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
