// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NEL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [Fact]
        [Trait("Category", "Witness")]
        public async Task ShouldRetrieveChunckedMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "WITNESS TEST";


            int targetSizeInMegabytes = 21; // You can adjust this value as needed
            int bytesPerMegabyte = 1024 * 1024;
            int targetSizeInBytes = targetSizeInMegabytes * bytesPerMegabyte;
            string smallString = "9694116538, 9694116414,"; // Test Patients
            int repeatCount = targetSizeInBytes / Encoding.UTF8.GetByteCount(smallString);

            string content = string.Concat(Enumerable.Repeat(smallString, repeatCount));


            //string content = "9694116538, 9694116414"; //Test Patients
            string mexSubject = "WITNESS TEST -  ShouldRetrieveStringMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldRetrieveStringMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";

            Message randomMessage = ComposeMessage.CreateStringMessage(
                mexTo,
                mexWorkflowId,
                content,
                mexSubject,
                mexLocalId,
                mexFileName,
                mexContentChecksum,
                contentType,
                contentEncoding);

            Message expectedMessage = randomMessage;

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

            // when
            Message retrievedMessage =
                await this.meshClient.Mailbox.RetrieveMessageAsync(sendMessageResponse.MessageId);

            // then
            // output Chuncked filed
            retrievedMessage.MessageId.Should().BeEquivalentTo(sendMessageResponse.MessageId);
            retrievedMessage.FileContent.Should().BeEquivalentTo(expectedMessage.FileContent);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}
