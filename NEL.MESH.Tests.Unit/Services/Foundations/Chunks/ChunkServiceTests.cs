// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Moq;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Services.Foundations.Chunks;
using Tynamix.ObjectFiller;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Chunks
{
    public partial class ChunkServiceTests
    {
        private readonly Mock<IMeshConfigurationBroker> meshConfigurationBrokerMock;
        private readonly IChunkService chunkService;

        public ChunkServiceTests()
        {
            this.meshConfigurationBrokerMock = new Mock<IMeshConfigurationBroker>();
            this.chunkService = new ChunkService(meshConfigurationBroker: this.meshConfigurationBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        public static string GetRandomString(int chunkSizeInBytes, int chunkCount)
        {
            int additionalBytes = GetRandomNumber(min: 0, max: chunkSizeInBytes - 1);
            int bytesToGenerate = (chunkSizeInBytes * chunkCount) + additionalBytes;
            Random random = new Random();
            byte[] randomBytes = new byte[bytesToGenerate];
            random.NextBytes(randomBytes);
            string randomString = Convert.ToBase64String(randomBytes);
            int excessBytes = randomString.Length % 4;

            if (excessBytes != 0)
            {
                randomString += new string('=', 4 - excessBytes);
            }

            return randomString;
        }

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static Message CreateRandomSendMessage(string stringContent)
        {
            var message = CreateMessageFiller().Create();
            message.Headers.Add("Content-Type", new List<string> { "text/plain" });
            message.Headers.Add("Mex-LocalID", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Subject", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Content-Checksum", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Content-Encrypted", new List<string> { "encrypted" });
            message.Headers.Add("Mex-From", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-To", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-WorkflowID", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-FileName", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Encoding", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Chunk-Range", new List<string> { GetRandomString() });
            message.StringContent = stringContent;
            message.FileContent = null;

            return message;
        }

        private static Message CreateRandomSendFileMessage(byte[] byteArrayContent)
        {
            var message = CreateMessageFiller().Create();
            message.Headers.Add("Content-Type", new List<string> { "text/plain" });
            message.Headers.Add("Mex-LocalID", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Subject", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Content-Checksum", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Content-Encrypted", new List<string> { "encrypted" });
            message.Headers.Add("Mex-From", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-To", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-WorkflowID", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-FileName", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Encoding", new List<string> { GetRandomString() });
            message.Headers.Add("Mex-Chunk-Range", new List<string> { GetRandomString() });
            message.FileContent = byteArrayContent;
            message.StringContent = null;

            return message;
        }

        private static Message CreateRandomMessage() =>
            CreateMessageFiller().Create();

        private static Filler<Message> CreateMessageFiller()
        {
            var filler = new Filler<Message>();
            filler.Setup()
                .OnProperty(message => message.Headers).Use(new Dictionary<string, List<string>>());

            return filler;
        }
    }
}
