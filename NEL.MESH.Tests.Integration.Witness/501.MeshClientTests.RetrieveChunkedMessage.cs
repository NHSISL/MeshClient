// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [WitnessTestsFact(DisplayName = "501 - Retrieving Chunked Messages")]
        [Trait("Category", "Witness")]
        public async Task ShouldRetrieveChunckedMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "WITNESS TEST - CHUNKING";
            int targetSizeInMegabytes = 1; //Over 1 will result in Chunking.
            string content = GetFileWithXBytes(targetSizeInMegabytes);
            string mexSubject = "WITNESS TEST -  ShouldRetrieveChunckedMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldRetrieveChunckedMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";
            byte[] contentBytes = Encoding.ASCII.GetBytes(content);

            using MemoryStream sendStream = new MemoryStream(contentBytes);

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    sendStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

            string messageId = sendMessageResponse.MessageId;
            using MemoryStream outputStream = new MemoryStream();

            // when
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync(messageId, outputStream);

            // then
            var fileName = retrievedMessage.Headers
                    .FirstOrDefault(h => h.Key == "mex-filename")
                        .Value.FirstOrDefault();

            fileName.Should().BeEquivalentTo(mexFileName);
            outputStream.ToArray().Should().BeEquivalentTo(contentBytes);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(retrievedMessage.MessageId);
        }

        [WitnessTestsFact(DisplayName = "501 - Retrieving Chunked MD5 Messages")]
        [Trait("Category", "Witness")]
        public async Task ShouldRetrieveChunckedMD5MessageAsync()
        {
            // given
            var expectedcheck = "634af5ebb487856f31f37018601b19de";
            using MemoryStream outputStream = new MemoryStream();

            // when
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync("20231124101605283253_EC7D79", outputStream);

            // then
            string md5Checksum = GetMD5Checksum(outputStream.ToArray());
            md5Checksum.Should().BeEquivalentTo(expectedcheck);

            await this.meshClient.Mailbox.AcknowledgeMessageAsync(retrievedMessage.MessageId);
        }
    }
}