// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [Fact]
        [Trait("Category", "Witness")]
        public async Task ShouldErrorSendMessageUndeliveredMessageAsync()
        {
            // given
            // Change MailboxId and Password to match deadletter mailbox
            string invalidMessageId = "20231117125902185257_995DE8";

            // when

            Message retrievedMessage =
                    await this.meshClient.Mailbox.RetrieveMessageAsync(invalidMessageId);

            // then
            retrievedMessage.MessageId.Should().BeEquivalentTo(invalidMessageId);
        }
    }
}
