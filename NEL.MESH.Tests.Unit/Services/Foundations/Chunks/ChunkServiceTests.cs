// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static string GetRandomString(int bytesToGenerate)
        {
            return new MnemonicString(
                wordCount: 1,
                wordMinLength: bytesToGenerate,
                wordMaxLength: bytesToGenerate).GetValue();
        }

        public static List<string> GetChunks(string content, int chunkSizeInBytes)
        {
            string stringContent = content;
            List<string> chunkedContent = new List<string>();
            int chunkSize = chunkSizeInBytes;

            for (int i = 0; i < stringContent.Length; i += chunkSize)
            {
                chunkSize = Math.Min(chunkSizeInBytes, stringContent.Length - i);
                string chunk = stringContent.Substring(i, chunkSize);
                chunkedContent.Add(chunk);
            }

            return chunkedContent;
        }

        private static void SetMexChunkRange(Message message, int item, int itemCount)
        {
            if (message.Headers.ContainsKey("Mex-Chunk-Range"))
            {
                message.Headers["Mex-Chunk-Range"] = new List<string> { $"{item}:{itemCount}" };
            }
            else
            {
                message.Headers.Add("Mex-Chunk-Range", new List<string> { $"{item}:{itemCount}" });
            }
        }

        public static byte[] GetRandomBytes(int bytesToGenerate)
        {
            Random random = new Random();
            int maxCharacters = bytesToGenerate / Encoding.UTF8.GetMaxByteCount(1);

            string randomString = new string(Enumerable.Range(0, maxCharacters)
                .Select(_ => (char)random.Next(0x80, 0x7FF)).ToArray());

            byte[] buffer = Encoding.UTF8.GetBytes(randomString);

            return buffer;
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
            message.MessageId = null;

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
            message.MessageId = null;

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
