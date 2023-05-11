// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Foundations.Chunks
{
    internal partial class ChunkService : IChunkService
    {
        private readonly IMeshConfigurationBroker meshConfigurationBroker;

        public ChunkService(IMeshConfigurationBroker meshConfigurationBroker)
        {
            this.meshConfigurationBroker = meshConfigurationBroker;
        }

        public List<Message> SplitMessageIntoChunks(Message message) =>
            TryCatch(() =>
            {
                ValidateMessageIsNotNull(message);
                int maxPartSize = this.meshConfigurationBroker.MaxChunkSizeInBytes;

                if (string.IsNullOrEmpty(message.StringContent))
                {
                    SetMexChunkRange(message, item: 1, itemCount: 1);

                    return new List<Message> { message };
                }

                List<string> parts = GetChunkedContent(message, maxPartSize);
                List<Message> chunkedMessages = ComposeNewMessagesFromChunks(message, parts);

                return chunkedMessages;
            });

        public List<Message> SplitFileMessageIntoChunks(Message message) =>
            TryCatch(() =>
            {
                ValidateMessageIsNotNull(message);
                int maxPartSize = this.meshConfigurationBroker.MaxChunkSizeInBytes;

                if (message.FileContent.Length <= maxPartSize)
                {
                    SetMexChunkRange(message, item: 1, itemCount: 1);

                    return new List<Message> { message };
                }

                List<byte[]> parts = GetChunkedByteArrayContent(message, maxPartSize);
                List<Message> chunkedMessages = ComposeNewFileMessagesFromChunks(message, parts);

                return chunkedMessages;
            });

        public Message CombineChunkedMessages(List<Message> chunks) =>
            throw new System.NotImplementedException();

        public Message CombineChunkedFileMessages(List<Message> chunks) =>
            throw new System.NotImplementedException();

        private static List<Message> ComposeNewMessagesFromChunks(Message message, List<string> parts)
        {
            List<Message> chunkedMessages = new List<Message>();

            for (int i = 0; i < parts.Count; i++)
            {
                Message chunk = new Message
                {
                    Headers = message.Headers,
                    StringContent = parts[i]
                };

                SetMexChunkRange(chunk, item: i + 1, itemCount: parts.Count);
                chunkedMessages.Add(chunk);
            }

            return chunkedMessages;
        }

        private static List<Message> ComposeNewFileMessagesFromChunks(Message message, List<byte[]> parts)
        {
            List<Message> chunkedMessages = new List<Message>();

            for (int i = 0; i < parts.Count; i++)
            {
                Message chunk = new Message
                {
                    Headers = message.Headers,
                    FileContent = parts[i]
                };

                SetMexChunkRange(chunk, item: i + 1, itemCount: parts.Count);
                chunkedMessages.Add(chunk);
            }

            return chunkedMessages;
        }

        private static List<string> GetChunkedContent(Message message, int chunkSizeInBytes)
        {
            string content = message.StringContent;
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            List<string> chunkedContent = new List<string>();

            for (int i = 0; i < bytes.Length; i += chunkSizeInBytes)
            {
                int chunkSize = Math.Min(chunkSizeInBytes, bytes.Length - i);
                byte[] chunkBytes = new byte[chunkSize];
                Array.Copy(bytes, i, chunkBytes, 0, chunkSize);
                string chunk = Encoding.UTF8.GetString(chunkBytes);
                chunkedContent.Add(chunk);
            }

            return chunkedContent;
        }

        private static List<byte[]> GetChunkedByteArrayContent(Message message, int chunkSizeInBytes)
        {
            byte[] byteContent = message.FileContent;
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
    }
}
