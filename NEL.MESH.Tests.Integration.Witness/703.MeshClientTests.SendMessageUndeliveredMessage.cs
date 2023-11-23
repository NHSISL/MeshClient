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
        [Fact(DisplayName = "703 - Send Message - Undelivered Message")]
        [Trait("Category", "Witness")]
        public async Task ShouldErrorSendMessageUndeliveredMessageAsync()
        {
            // given
            // Change MailboxId and Password to match deadletter mailbox

            // MailboxId = "QMFOT005",
            // Password = "3k2JZOTyQboi",

            // What i had as the messageId originally in mailbox 17/11/2023 1259
            string invalidMessageId = "20231117125902185257_995DE8";

            // Below is messageId In current DeadLetter 22/11/2023 16:16
            // string invalidMessageId = "20231122161608316585_56338A";

            // Below is LinkedMessageId 17/11/2023 13:53
            // string invalidMessageId = "20231117135329486670_0EC1B3";

            // when
            Message retrievedMessage =
                    await this.meshClient.Mailbox.RetrieveMessageAsync(invalidMessageId);

            // then
            retrievedMessage.MessageId.Should().BeEquivalentTo(invalidMessageId);
        }
    }
}
