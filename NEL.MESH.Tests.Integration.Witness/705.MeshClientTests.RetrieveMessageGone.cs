// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using NEL.MESH.Clients;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [Fact(DisplayName = "705 - Download Message - Gone Message, Message already Downloaded")]
        [Trait("Category", "Witness")]
        public async Task ShouldNotRetirveGoneMessageAsync()
        {
            // given
            var config = this.meshConfigurations.DeepClone();
            config.MailboxId = "QMFOT005";
            config.Password = "3k2JZOTyQboi";
            var client = new MeshClient(meshConfigurations: config);

            string invalidMessageId = "20231122161608316585_56338A";
            //string invalidMessageId = "20231123151625288395_ADB038";

            // when
            Message retrievedMessage =
                    await client.Mailbox.RetrieveMessageAsync(invalidMessageId);

            // then
        }
    }
}
