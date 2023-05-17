// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration
{
    public partial class MeshClientTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldAcknowledgeMessageAsync()
        {
            // given
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "INTEGRATION TEST";
            byte[] fileContent = Encoding.ASCII.GetBytes(GetRandomString());
            string mexContentEncrypted = GetRandomString();
            string mexSubject = "INTEGRATION TEST -  ShouldAcknowledgeMessageAsync";
            string mexLocalId = GetRandomString();
            string mexFileName = $"ShouldAcknowledgeMessageAsync.csv";
            string mexContentChecksum = GetRandomString();
            string contentType = "application/octet-stream";
            string contentEncoding = GetRandomString();

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexContentEncrypted,
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
