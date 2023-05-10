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
                List<Message> chunkedMessages = new List<Message>();
                ComposeNewMessagesFromChunks(message, parts, chunkedMessages);

                return chunkedMessages;
            });

        public List<Message> SplitFileMessageIntoChunks(Message message) =>
            throw new System.NotImplementedException();

        public Message CombineChunkedMessages(List<Message> chunks) =>
            throw new System.NotImplementedException();

        public Message CombineChunkedFileMessages(List<Message> chunks) =>
            throw new System.NotImplementedException();

        private static void ComposeNewMessagesFromChunks(Message message, List<string> parts, List<Message> chunkedMessages)
        {
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
        }

        private static List<string> GetChunkedContent(Message message, int chunkSizeInBytes)
        {
            byte[] byteContent = Encoding.UTF8.GetBytes(message.StringContent);
            List<string> chunkedContent = new List<string>();

            for (int i = 0; i < byteContent.Length; i += chunkSizeInBytes)
            {
                int chunkSize = Math.Min(chunkSizeInBytes, byteContent.Length - i);
                byte[] chunk = new byte[chunkSize];
                Array.Copy(byteContent, i, chunk, 0, chunkSize);
                chunkedContent.Add(Encoding.UTF8.GetString(chunk));
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
