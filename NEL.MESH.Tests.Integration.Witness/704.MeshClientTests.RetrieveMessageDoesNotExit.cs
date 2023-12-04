// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [WitnessTestsFact(DisplayName = "704 - Download Message - Message Does Not Exist")]
        [Trait("Category", "Witness")]
        public async Task ShouldRetrieveStringMessageDoesNotExistAsync()
        {
            // given
            string invalidMessageId = "thisisaninvalidmessageid1234567890";
            List<string> statusCode = new List<string> { "404 - Not Found" };
            List<string> errorEvent = new List<string> { "RECEIVE" };
            List<string> errorCode = new List<string> { "20" };
            List<string> errorDescription = new List<string> { "Message does not exist" };

            // when
            ValueTask<Message> retrievedMessageTask =
                    this.meshClient.Mailbox.RetrieveMessageAsync(invalidMessageId);

            MeshClientValidationException actualMeshClientValidationException =
               await Assert.ThrowsAsync<MeshClientValidationException>(retrievedMessageTask.AsTask);

            // then
            actualMeshClientValidationException.Data.Count.Should().Be(5);
            actualMeshClientValidationException.Data["StatusCode"].Should().BeEquivalentTo(statusCode);
            actualMeshClientValidationException.Data["ErrorEvent"].Should().BeEquivalentTo(errorEvent);
            actualMeshClientValidationException.Data["ErrorCode"].Should().BeEquivalentTo(errorCode);
            actualMeshClientValidationException.Data["ErrorDescription"].Should().BeEquivalentTo(errorDescription);
            actualMeshClientValidationException.Data["MessageId"].Should().NotBeNull();
        }
    }
}
