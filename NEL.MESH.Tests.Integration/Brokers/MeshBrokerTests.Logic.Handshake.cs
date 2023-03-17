// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace NEL.MESH.Tests.Integration.Brokers
{
    public partial class MeshBrokerTests
    {
        [Fact]
        public async Task ShouldDoHandshakeAsync()
        {
            // given
            // when
            HttpResponseMessage response = await this.meshBroker.HandshakeAsync();
            var body = await response.Content.ReadAsStringAsync();

            // then
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            body.Should().BeEquivalentTo($"{{\"mailboxId\":\"{this.meshApiConfiguration.MailboxId}\"}}");
        }
    }
}