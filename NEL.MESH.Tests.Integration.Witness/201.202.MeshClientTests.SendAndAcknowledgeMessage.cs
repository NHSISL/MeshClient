// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldSendAndAcknowledgeMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "WHITNESS TEST";
            string content = "9694116538, 9694116414"; //Test Patients
            string mexSubject = "WHITNESS TEST -  ShouldSendAndAckMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldSendAndAckMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";

            // when
            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    content,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

            // then
            sendMessageResponse.MessageId.Should().NotBeNullOrEmpty();
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}
