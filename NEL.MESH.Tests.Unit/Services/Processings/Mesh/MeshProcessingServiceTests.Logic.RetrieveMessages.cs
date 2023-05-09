// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveMessagesAsync()
        {
            // given
            string authorizationToken = GetRandomString();
            List<string> randomStringList = GetRandomStringList();
            List<string> outputStringList = randomStringList;
            List<string> expectedStringList = outputStringList.DeepClone();

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessagesAsync(authorizationToken))
                    .ReturnsAsync(outputStringList);

            // when
            var actualStringList = await this.meshProcessingService.RetrieveMessagesAsync(authorizationToken);

            // then
            actualStringList.Should().BeEquivalentTo(expectedStringList);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessagesAsync(authorizationToken),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
