// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Force.DeepCloner;
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

                if (message.FileContent.Length <= maxPartSize)
                {
                    SetMexChunkRange(message, item: 1, itemCount: 1);

                    return new List<Message> { message };
                }

                List<byte[]> parts = GetChunkedByteArrayContent(message, maxPartSize);
                List<Message> chunkedMessages = ComposeNewMessagesFromChunks(message, parts);

                return chunkedMessages;
            });

        private static List<Message> ComposeNewMessagesFromChunks(Message message, List<byte[]> parts)
        {
            List<Message> chunkedMessages = new List<Message>();

            for (int i = 0; i < parts.Count; i++)
            {
                Message chunk = new Message
                {
                    Headers = message.Headers.DeepClone(),
                    FileContent = parts[i]
                };

                SetMexChunkRange(chunk, item: i + 1, itemCount: parts.Count);
                chunkedMessages.Add(chunk);
            }

            return chunkedMessages;
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
            if (message.Headers.ContainsKey("mex-chunk-range"))
            {
                message.Headers["mex-chunk-range"] = new List<string> { $"{item}:{itemCount}" };
            }
            else
            {
                message.Headers.Add("mex-chunk-range", new List<string> { $"{item}:{itemCount}" });
            }
        }
    }
}
