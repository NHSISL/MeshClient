// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
            byte[] generatedBytes = GetRandomBytes(bytesToGenerate);
            string randomString = Encoding.UTF8.GetString(generatedBytes);

            return randomString;
        }

        public static List<string> GetChunks(string content, int chunkSizeInBytes)
        {
            if (Encoding.UTF8.GetByteCount(content) <= chunkSizeInBytes)
            {
                return new List<string> { content };
            }

            byte[] bytes = Encoding.UTF8.GetBytes(content);
            List<string> chunkedContent = new List<string>();

            for (int i = 0; i < bytes.Length;)
            {
                int chunkSize = Math.Min(chunkSizeInBytes, bytes.Length - i);
                byte[] chunkBytes = new byte[chunkSize];
                Array.Copy(bytes, i, chunkBytes, 0, chunkSize);
                string chunk = Encoding.UTF8.GetString(chunkBytes);

                while (Encoding.UTF8.GetByteCount(chunk) > chunkSizeInBytes)
                {
                    chunkSize--;
                    chunkBytes = new byte[chunkSize];
                    Array.Copy(bytes, i, chunkBytes, 0, chunkSize);
                    chunk = Encoding.UTF8.GetString(chunkBytes);
                }

                i += chunkSize;
                chunkedContent.Add(chunk);
            }

            return chunkedContent;
        }

        private static List<byte[]> GetChunkedByteArrayContent(byte[] byteContent, int chunkSizeInBytes)
        {
            List<byte[]> chunkedContent = new List<byte[]>();

            for (int i = 0; i < byteContent.Length; i += chunkSizeInBytes)
            {
                int chunkSize = Math.Min(chunkSizeInBytes, byteContent.Length - i);
                byte[] chunk = new byte[chunkSize];
                Array.Copy(byteContent, i, chunk, 0, chunkSize);
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
            byte[] buffer = new byte[bytesToGenerate];

            Random random = new Random();
            random.NextBytes(buffer);

            return buffer;
        }

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static Message CreateRandomSendMessage(byte[] byteArrayContent)
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
            message.FileContent = null;
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
