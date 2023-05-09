// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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
    }
}
