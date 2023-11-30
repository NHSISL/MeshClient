// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [Fact(DisplayName = "703 - Send Message - Undelivered Message DEAD LETTER")]
        [Trait("Category", "Witness")]
        public async Task ShouldErrorSendMessageUndeliveredMessageAsync()
        {
            // Commented Out as no dead letter is available
            // Left in if we ever have to test again

            //// given
            //var config = this.meshConfigurations.DeepClone();
            //config.MailboxId = "QMFOT005";
            //config.Password = "3k2JZOTyQboi";
            //var client = new MeshClient(meshConfigurations: config);

            //string status = "ERROR";
            //string statusDescription = "Message not collected by recipient after 5 days";
            //string messageType = "REPORT";

            ////string invalidMessageId = "20231122161608316585_56338A";
            //string invalidMessageId = "20231117125902185257_995DE8";

            //// when
            //Message retrievedMessage =
            //        await client.Mailbox.RetrieveMessageAsync(invalidMessageId);

            //// then
            //string actualStatus = retrievedMessage.Headers["mex-statussuccess"].FirstOrDefault();
            //string actualStatusDescription = retrievedMessage.Headers["mex-statusdescription"].FirstOrDefault();
            //string actualMessageType = retrievedMessage.Headers["mex-messagetype"].FirstOrDefault();

            //actualStatus.Should().BeEquivalentTo(status);
            //actualStatusDescription.Should().BeEquivalentTo(statusDescription);
            //actualMessageType.Should().BeEquivalentTo(messageType);
        }
    }
}
