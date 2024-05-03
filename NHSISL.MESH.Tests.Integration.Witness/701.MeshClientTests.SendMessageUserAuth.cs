// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using NHSISL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NHSISL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [WitnessTestsFact(DisplayName = "701 - User Authentication Disabled Mailbox")]
        [Trait("Category", "Witness")]
        public async Task ShouldErrorSendMessageUserAuthAsync()
        {
            // given
            //Connecting system's mailbox disabled through MOLES (by HSCIC)

            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "WHITNESS TEST";
            string content = "9694116538, 9694116414"; //Test Patients
            string mexSubject = "WHITNESS TEST -  ShouldSendMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldSendMessageAsync.csv";
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
