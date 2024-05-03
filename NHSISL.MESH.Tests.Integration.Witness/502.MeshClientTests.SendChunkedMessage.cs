// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NHSISL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NHSISL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [WitnessTestsFact(DisplayName = "502 - Sending Chunked Message")]
        [Trait("Category", "Witness")]
        public async Task ShouldSendChunckedMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "WITNESS TEST - CHUNKING";
            int targetSizeInMegabytes = 2; //Over 1 will result in Chunking.
            string content = GetFileWithXBytes(targetSizeInMegabytes);
            string mexSubject = "WITNESS TEST -  ShouldRetrieveChunckedMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldRetrieveChunckedMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";

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

            string messageId = sendMessageResponse.MessageId;

            // when
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync(messageId);

            // then

            var fileName = retrievedMessage.Headers
                    .FirstOrDefault(h => h.Key == "mex-filename")
                        .Value.FirstOrDefault();

            var contentBytes =
                    Encoding.ASCII.GetBytes(content);

            fileName.Should().BeEquivalentTo(mexFileName);
            retrievedMessage.FileContent.Should().BeEquivalentTo(contentBytes);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(retrievedMessage.MessageId);
        }
    }
}
