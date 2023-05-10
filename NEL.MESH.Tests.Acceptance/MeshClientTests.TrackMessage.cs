// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using NEL.MESH.Models.Foundations.Mesh;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class MeshClientTests
    {
        [Fact(Skip = "Excluded")]
        [Trait("Category", "Acceptance")]
        public async Task ShouldTrackMessageAsync()
        {
            // given
            string messageId = GetRandomString();
            string path = $"/messageexchange/{this.meshConfigurations.MailboxId}/outbox/tracking";

            TrackingInfo randomTrackingInfo = new TrackingInfo
            {
                AddressType = GetRandomString(),
                Checksum = GetRandomString(),
                ChunkCount = GetRandomNumber(),
                CompressFlag = GetRandomString(),
                DownloadTimestamp = GetRandomString(),
                DtsId = GetRandomString(),
                EncryptedFlag = GetRandomString(),
                ExpiryTime = GetRandomString(),
                FileName = GetRandomString(),
                FileSize = GetRandomNumber(),
                IsCompressed = GetRandomString(),
                LocalId = GetRandomString(),
                MeshRecipientOdsCode = GetRandomString(),
                MessageId = GetRandomString(),
                MessageType = GetRandomString(),
                PartnerId = GetRandomString(),
                Recipient = GetRandomString(),
                RecipientName = GetRandomString(),
                RecipientOrgCode = GetRandomString(),
                RecipientSmtp = GetRandomString(),
                Sender = GetRandomString(),
                SenderName = GetRandomString(),
                SenderOdsCode = GetRandomString(),
                SenderOrgCode = GetRandomString(),
                SenderSmtp = GetRandomString(),
                Status = GetRandomString(),
                StatusSuccess = GetRandomString(),
                UploadTimestamp = GetRandomString(),
                Version = GetRandomString(),
                WorkflowId = GetRandomString(),
            };

            string serialisedResponseMessage = JsonConvert.SerializeObject(randomTrackingInfo);

            Message outputMessage = new Message
            {
                MessageId = messageId,
                TrackingInfo = randomTrackingInfo
            };

            Message expectedTrackMessageResult = outputMessage;

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingGet()
                        .WithHeader("Mex-ClientVersion", this.meshConfigurations.MexClientVersion)
                        .WithHeader("Mex-OSName", this.meshConfigurations.MexOSName)
                        .WithHeader("Mex-OSVersion", this.meshConfigurations.MexOSVersion)
                        .WithHeader("Authorization", "*", WireMock.Matchers.MatchBehaviour.AcceptOnMatch)
                        .WithParam("messageID", messageId)
                        )
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithBody(serialisedResponseMessage));

            // when
            Message actualTrackMessageResult = await this.meshClient.Mailbox.TrackMessageAsync(messageId);

            // then
            actualTrackMessageResult.Should().BeEquivalentTo(expectedTrackMessageResult);
        }
    }
}
