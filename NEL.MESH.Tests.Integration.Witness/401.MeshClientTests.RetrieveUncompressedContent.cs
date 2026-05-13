// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [WitnessTestsFact(DisplayName = "401 - Retrieve Uncompressed Content")]
        [Trait("Category", "Witness")]
        public async Task ShouldRetrieveStringMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "401 WITNESS TEST";
            string content = "9694116538, 9694116414"; //Test Patients
            string mexSubject = "WITNESS TEST -  ShouldRetrieveStringMessageUncompressed";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldRetrieveStringMessageUncompressed.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";
            byte[] expectedBytes = Encoding.UTF8.GetBytes(content);

            using MemoryStream sendStream = new MemoryStream(expectedBytes);

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

            using MemoryStream outputStream = new MemoryStream();

            // when
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync(sendMessageResponse.MessageId, outputStream);

            // then
            retrievedMessage.MessageId.Should().BeEquivalentTo(sendMessageResponse.MessageId);
            outputStream.ToArray().Should().BeEquivalentTo(expectedBytes);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}