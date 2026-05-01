// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using Force.DeepCloner;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Models.Foundations.Chunking.Exceptions;
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
                content.Seek(0, SeekOrigin.Begin);
                int maxPartSize = this.meshConfigurationBroker.MaxChunkSizeInBytes;
                ValidateMaxChunkSize(maxPartSize);
                long totalChunksLong = (long)Math.Ceiling((double)content.Length / maxPartSize);
                int totalChunks = (int)Math.Min(totalChunksLong, int.MaxValue);
                totalChunks = Math.Max(totalChunks, 1);

                return ReadChunksFromStream(messageTemplate, content, maxPartSize, totalChunks);
            });

        private static IEnumerable<(Message Message, byte[] Content)> ReadChunksFromStream(
            Message messageTemplate,
            Stream content,
            int maxPartSize,
            int totalChunks)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(maxPartSize);

            try
            {
                for (int chunkIndex = 0; chunkIndex < totalChunks; chunkIndex++)
                {
                    int bytesRead = 0;

                    try
                    {
                        while (bytesRead < maxPartSize)
                        {
                            int read = content.Read(buffer, bytesRead, maxPartSize - bytesRead);

                            if (read == 0)
                            {
                                break;
                            }

                            bytesRead += read;
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new InvalidStreamChunkException(
                            message: $"Failed to read chunk {chunkIndex + 1} from stream: {exception.Message}");
                    }

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
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
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
