// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [Fact]
        [Trait("Category", "Witness")]
        public async Task ShouldRetrieveStringMessageDoesNotExistAsync()
        {
            // given
            string invalidMessageId = "thisisaninvalidmessageid1234567890";

            // when

            Message retrievedMessage =
                    await this.meshClient.Mailbox.RetrieveMessageAsync(invalidMessageId);

            // then
            string t = null;
        }
    }
}
