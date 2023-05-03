﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldGetMessagesAsync()
        {
            // given
            Message randomMessage = CreateRandomSendMessage(
                mexFrom: this.meshConfigurations.MailboxId,
                mexTo: this.meshConfigurations.MailboxId,
                mexWorkflowId: "INTEGRATION TEST",
                mexLocalId: GetRandomString(),
                mexSubject: "INTEGRATION TEST -  ShouldGetMessagesAsync",
                mexFileName: $"ShouldGetMessagesAsync.csv",
                mexContentChecksum: null,
                mexContentEncrypted: null,
                mexEncoding: null,
                mexChunkRange: null,
                contentType: "text/plain",
                content: GetRandomString());

            Message sendMessageResponse =
                await this.meshClient.Mailbox.SendMessageAsync(randomMessage);

            // when
            List<string> messageList = await this.meshClient.Mailbox.RetrieveMessagesAsync();

            // then
            messageList.Should().Contain(sendMessageResponse.MessageId);
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(sendMessageResponse.MessageId);
        }
    }
}
