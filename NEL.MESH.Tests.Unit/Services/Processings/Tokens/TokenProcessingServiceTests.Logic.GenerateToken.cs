// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Token
{
    public partial class TokenProcessingServiceTests
    {
        [Fact]
        public async Task ShouldGenerateTokenAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            string expectedResult = authorizationToken;

            this.tokenServiceMock.Setup(service =>
                service.GenerateTokenAsync())
                    .ReturnsAsync(authorizationToken);

            // when
            var actualResult = await this.tokenProcessingService.GenerateTokenAsync();

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.tokenServiceMock.Verify(service =>
                service.GenerateTokenAsync(),
                    Times.Once);

            this.tokenServiceMock.VerifyNoOtherCalls();
        }
    }
}
