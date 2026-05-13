// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using FluentAssertions;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Foundations.Mesh;
using Tynamix.ObjectFiller;
using Xunit;

namespace NEL.MESH.Tests.Unit.Clients.Mailboxes
{
    public class ComposeMessageTests
    {
        private static string GetRandomString() =>
            new MnemonicString(wordCount: 1, wordMinLength: 2, wordMaxLength: 20).GetValue();

        [Fact]
        public void ShouldAddAcceptHeaderWhenAcceptIsProvided()
        {
            // given
            string randomAccept = "application/json";
            string randomTo = GetRandomString();
            string randomWorkflowId = GetRandomString();
            string emptyContentEncoding = "";

            // when
            Message message = ComposeMessage.CreateMessage(
                mexTo: randomTo,
                mexWorkflowId: randomWorkflowId,
                accept: randomAccept,
                contentEncoding: emptyContentEncoding);

            // then
            message.Headers.Should().ContainKey("accept");
            message.Headers["accept"].Should().BeEquivalentTo(new List<string> { randomAccept });
        }

        [Fact]
        public void ShouldAddAcceptHeaderEvenWhenContentEncodingIsEmpty()
        {
            // given
            string randomAccept = "application/json";
            string randomTo = GetRandomString();
            string randomWorkflowId = GetRandomString();

            // when
            Message message = ComposeMessage.CreateMessage(
                mexTo: randomTo,
                mexWorkflowId: randomWorkflowId,
                contentEncoding: "",
                accept: randomAccept);

            // then
            message.Headers.Should().ContainKey("accept");
            message.Headers["accept"].Should().BeEquivalentTo(new List<string> { randomAccept });
        }

        [Fact]
        public void ShouldNotAddAcceptHeaderWhenAcceptIsEmpty()
        {
            // given
            string randomTo = GetRandomString();
            string randomWorkflowId = GetRandomString();

            // when
            Message message = ComposeMessage.CreateMessage(
                mexTo: randomTo,
                mexWorkflowId: randomWorkflowId,
                accept: "");

            // then
            message.Headers.Should().NotContainKey("accept");
        }

        [Fact]
        public void ShouldAddRequiredHeadersForMinimalMessage()
        {
            // given
            string randomTo = GetRandomString();
            string randomWorkflowId = GetRandomString();

            // when
            Message message = ComposeMessage.CreateMessage(
                mexTo: randomTo,
                mexWorkflowId: randomWorkflowId);

            // then
            message.Headers.Should().ContainKey("mex-to");
            message.Headers["mex-to"].Should().BeEquivalentTo(new List<string> { randomTo });
            message.Headers.Should().ContainKey("mex-workflowid");
            message.Headers["mex-workflowid"].Should().BeEquivalentTo(new List<string> { randomWorkflowId });
        }

        [Fact]
        public void ShouldAddAllOptionalHeadersWhenProvided()
        {
            // given
            string randomTo = GetRandomString();
            string randomWorkflowId = GetRandomString();
            string randomSubject = GetRandomString();
            string randomLocalId = GetRandomString();
            string randomFileName = GetRandomString();
            string randomChecksum = GetRandomString();
            string randomContentType = "application/octet-stream";
            string randomContentEncoding = "gzip";
            string randomAccept = "application/json";

            // when
            Message message = ComposeMessage.CreateMessage(
                mexTo: randomTo,
                mexWorkflowId: randomWorkflowId,
                mexSubject: randomSubject,
                mexLocalId: randomLocalId,
                mexFileName: randomFileName,
                mexContentChecksum: randomChecksum,
                contentType: randomContentType,
                contentEncoding: randomContentEncoding,
                accept: randomAccept);

            // then
            message.Headers.Should().ContainKey("mex-subject");
            message.Headers["mex-subject"].Should().BeEquivalentTo(new List<string> { randomSubject });
            message.Headers.Should().ContainKey("mex-localid");
            message.Headers["mex-localid"].Should().BeEquivalentTo(new List<string> { randomLocalId });
            message.Headers.Should().ContainKey("mex-filename");
            message.Headers["mex-filename"].Should().BeEquivalentTo(new List<string> { randomFileName });
            message.Headers.Should().ContainKey("mex-content-checksum");
            message.Headers["mex-content-checksum"].Should().BeEquivalentTo(new List<string> { randomChecksum });
            message.Headers.Should().ContainKey("content-type");
            message.Headers["content-type"].Should().BeEquivalentTo(new List<string> { randomContentType });
            message.Headers.Should().ContainKey("content-encoding");
            message.Headers["content-encoding"].Should().BeEquivalentTo(new List<string> { randomContentEncoding });
            message.Headers.Should().ContainKey("accept");
            message.Headers["accept"].Should().BeEquivalentTo(new List<string> { randomAccept });
        }
    }
}
