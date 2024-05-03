﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using NHSISL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace NHSISL.MESH.Tests.Integration
{
    public partial class MeshClientTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldSendMessageAsync()
        {
            // given
            string mexTo = this.meshConfigurations.MailboxId;
            string mexWorkflowId = "INTEGRATION TEST";
            string content = GetRandomString();
            string mexSubject = "INTEGRATION TEST -  ShouldSendMessageAsync";
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
