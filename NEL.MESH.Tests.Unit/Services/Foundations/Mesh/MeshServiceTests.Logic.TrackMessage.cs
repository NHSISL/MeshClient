// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.ExternalModeld;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldTrackMessageAsync()
        {
            // given
            dynamic randomTrackingProperties = CreateRandomTrackingProperties();
            string randomString = GetRandomString();
            string inputMessageId = randomString;
            Message randomMessage = CreateRandomMessage();
            randomMessage.MessageId = inputMessageId;

            randomMessage.TrackingInfo =
                MapDynamicObjectToTrackingInfo(randomTrackingProperties);

            TrackMessageResponse trackMessageResponse =
                MapDynamicObjectToTrackMessageResponse(randomTrackingProperties);

            Message inputMessage = randomMessage;
            HttpResponseMessage responseMessage = CreateTrackingHttpResponseMessage(trackMessageResponse);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(inputMessageId))
                    .ReturnsAsync(responseMessage);

            Message expectedMessage = GetMessageFromTrackingHttpResponseMessage(inputMessageId, responseMessage);

            // when
            var actualMessage = await this.meshService.TrackMessageAsync(inputMessageId);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(inputMessageId),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        private TrackMessageResponse MapDynamicObjectToTrackMessageResponse(dynamic randomTrackingProperties)
        {
            return new TrackMessageResponse
            {
                AddressType = randomTrackingProperties.AddressType,
                Checksum = randomTrackingProperties.Checksum,
                ChunkCount = randomTrackingProperties.ChunkCount,
                CompressFlag = randomTrackingProperties.CompressFlag,
                DownloadTimestamp = randomTrackingProperties.DownloadTimestamp,
                DtsId = randomTrackingProperties.DtsId,
                EncryptedFlag = randomTrackingProperties.EncryptedFlag,
                ExpiryTime = randomTrackingProperties.ExpiryTime,
                FileName = randomTrackingProperties.FileName,
                FileSize = randomTrackingProperties.FileSize,
                IsCompressed = randomTrackingProperties.IsCompressed,
                LocalId = randomTrackingProperties.LocalId,
                MeshRecipientOdsCode = randomTrackingProperties.MeshRecipientOdsCode,
                MessageId = randomTrackingProperties.MessageId,
                MessageType = randomTrackingProperties.MessageType,
                PartnerId = randomTrackingProperties.PartnerId,
                Recipient = randomTrackingProperties.Recipient,
                RecipientName = randomTrackingProperties.RecipientName,
                RecipientOrgCode = randomTrackingProperties.RecipientOrgCode,
                RecipientSmtp = randomTrackingProperties.RecipientSmtp,
                Sender = randomTrackingProperties.Sender,
                SenderName = randomTrackingProperties.SenderName,
                SenderOdsCode = randomTrackingProperties.SenderOdsCode,
                SenderOrgCode = randomTrackingProperties.SenderOrgCode,
                SenderSmtp = randomTrackingProperties.SenderSmtp,
                Status = randomTrackingProperties.Status,
                StatusSuccess = randomTrackingProperties.StatusSuccess,
                UploadTimestamp = randomTrackingProperties.UploadTimestamp,
                Version = randomTrackingProperties.Version,
                WorkflowId = randomTrackingProperties.WorkflowId
            };
        }

        private TrackingInfo MapDynamicObjectToTrackingInfo(dynamic randomTrackingProperties)
        {
            return new TrackingInfo
            {
                AddressType = randomTrackingProperties.AddressType,
                Checksum = randomTrackingProperties.Checksum,
                ChunkCount = randomTrackingProperties.ChunkCount,
                CompressFlag = randomTrackingProperties.CompressFlag,
                DownloadTimestamp = randomTrackingProperties.DownloadTimestamp,
                DtsId = randomTrackingProperties.DtsId,
                EncryptedFlag = randomTrackingProperties.EncryptedFlag,
                ExpiryTime = randomTrackingProperties.ExpiryTime,
                FileName = randomTrackingProperties.FileName,
                FileSize = randomTrackingProperties.FileSize,
                IsCompressed = randomTrackingProperties.IsCompressed,
                LocalId = randomTrackingProperties.LocalId,
                MeshRecipientOdsCode = randomTrackingProperties.MeshRecipientOdsCode,
                MessageId = randomTrackingProperties.MessageId,
                MessageType = randomTrackingProperties.MessageType,
                PartnerId = randomTrackingProperties.PartnerId,
                Recipient = randomTrackingProperties.Recipient,
                RecipientName = randomTrackingProperties.RecipientName,
                RecipientOrgCode = randomTrackingProperties.RecipientOrgCode,
                RecipientSmtp = randomTrackingProperties.RecipientSmtp,
                Sender = randomTrackingProperties.Sender,
                SenderName = randomTrackingProperties.SenderName,
                SenderOdsCode = randomTrackingProperties.SenderOdsCode,
                SenderOrgCode = randomTrackingProperties.SenderOrgCode,
                SenderSmtp = randomTrackingProperties.SenderSmtp,
                Status = randomTrackingProperties.Status,
                StatusSuccess = randomTrackingProperties.StatusSuccess,
                UploadTimestamp = randomTrackingProperties.UploadTimestamp,
                Version = randomTrackingProperties.Version,
                WorkflowId = randomTrackingProperties.WorkflowId
            };
        }

        private dynamic CreateRandomTrackingProperties()
        {
            return new
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
                WorkflowId = GetRandomString()
            };
        }
    }
}
