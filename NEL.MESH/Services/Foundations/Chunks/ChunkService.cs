// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
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

        public IEnumerable<(Message message, byte[] content)> SplitStreamIntoChunks(
            Message messageTemplate,
            Stream content) =>
            TryCatch(() =>
            {
                ValidateMessageIsNotNull(messageTemplate);
                ValidateStream(content);
                int maxPartSize = this.meshConfigurationBroker.MaxChunkSizeInBytes;
                int totalChunks = (int)Math.Ceiling((double)content.Length / maxPartSize);
                totalChunks = Math.Max(totalChunks, 1);

                return ReadChunksFromStream(messageTemplate, content, maxPartSize, totalChunks);
            });

        private static IEnumerable<(Message Message, byte[] Content)> ReadChunksFromStream(
            Message messageTemplate,
            Stream content,
            int maxPartSize,
            int totalChunks)
        {
            byte[] buffer = new byte[maxPartSize];

            for (int chunkIndex = 0; chunkIndex < totalChunks; chunkIndex++)
            {
                int bytesRead = content.Read(buffer, 0, maxPartSize);
                byte[] chunkData = new byte[bytesRead];
                Buffer.BlockCopy(buffer, 0, chunkData, 0, bytesRead);

                Message chunk = new Message
                {
                    Headers = messageTemplate.Headers.DeepClone()
                };

                SetMexChunkRange(chunk, item: chunkIndex + 1, itemCount: totalChunks);

                yield return (chunk, chunkData);
            }
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
