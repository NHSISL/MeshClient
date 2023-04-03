// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace NEL.MESH.Tests.Integration
{
    public partial class MeshClientTests
    {
        [Fact(Skip = "Skip test till secrets sorted")]
        public async Task ShouldDoHandshakeAsync()
        {
            // given
            bool expectedResult = true;

            // when
            bool actualResult = await this.meshClient.Mailbox.HandshakeAsync();

            // then
            actualResult.Should().Be(expectedResult);
        }
    }
}
