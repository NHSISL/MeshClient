// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
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
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "INTEGRATION TEST";
            byte[] fileContent = Encoding.ASCII.GetBytes(GetRandomString());
            string mexSubject = "INTEGRATION TEST -  ShouldAcknowledgeMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldAcknowledgeMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "application/octet-stream";
            string contentEncoding = "";

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

            // when
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
            List<string> messageList = await this.meshClient.Mailbox.RetrieveMessagesAsync();

            // then
            messageList.Should().NotContain(sendMessageResponse.MessageId);
        }
    }
}
