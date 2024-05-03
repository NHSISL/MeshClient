// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSISL.MESH.Models.Clients.Mesh.Exceptions;
using NHSISL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NHSISL.MESH.Tests.Integration.Witness
{
    public partial class MeshClientTests
    {
        [WitnessTestsFact(DisplayName = "702 - Send Message - Invalid Recipient")]
        [Trait("Category", "Witness")]
        public async Task ShouldErrorSendMessageInvalidRecipientAsync()
        {
            // given
            // string mexTo = this.meshConfigurations.MailboxId;
            string mexTo = "aninvalidid1234";
            string mexWorkflowId = "WHITNESS TEST";
            string content = "9694116538, 9694116414"; //Test Patients
            string mexSubject = "WHITNESS TEST -  ShouldSendMessageAsync";
            string mexLocalId = Guid.NewGuid().ToString();
            string mexFileName = $"ShouldSendMessageAsync.csv";
            string mexContentChecksum = Guid.NewGuid().ToString();
            string contentType = "text/plain";
            string contentEncoding = "";

            List<string> statusCode = new List<string> { "417 - Expectation Failed" };
            List<string> errorEvent = new List<string> { "SEND" };
            List<string> errorCode = new List<string> { "12" };
            List<string> errorDescription = new List<string> { "Unregistered to address" };

            // when
            ValueTask<Message> sendMessageTask =
                this.meshClient.Mailbox.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    content,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding);

            MeshClientValidationException actualMeshClientValidationException =
                await Assert.ThrowsAsync<MeshClientValidationException>(sendMessageTask.AsTask);

            // then

            actualMeshClientValidationException.Data.Count.Should().Be(5);
            actualMeshClientValidationException.Data["StatusCode"].Should().BeEquivalentTo(statusCode);
            actualMeshClientValidationException.Data["ErrorEvent"].Should().BeEquivalentTo(errorEvent);
            actualMeshClientValidationException.Data["ErrorCode"].Should().BeEquivalentTo(errorCode);
            actualMeshClientValidationException.Data["ErrorDescription"].Should().BeEquivalentTo(errorDescription);
            actualMeshClientValidationException.Data["MessageId"].Should().NotBeNull();
            var messageId = ((List<string>)actualMeshClientValidationException.Data["MessageId"]).First();

            List<string> messageIds = await this.meshClient.Mailbox.RetrieveMessagesAsync();
            foreach (var item in messageIds)
            {
                var msg = await this.meshClient.Mailbox.RetrieveMessageAsync(item);
                List<string> linkedId = new List<string>();
                msg.Headers.TryGetValue("mex-linkedmsgid", out linkedId);

                if (linkedId.FirstOrDefault() == messageId)
                {
                    string reportId = msg.Headers["mex-messageid"].FirstOrDefault();
                    await this.meshClient.Mailbox.AcknowledgeMessageAsync(reportId);
                    break;
                }
            }
        }
    }
}
