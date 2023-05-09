// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
            throw new System.NotImplementedException();

        public List<Message> SplitFileMessageIntoChunks(Message message) =>
            throw new System.NotImplementedException();

        public Message CombineChunkedMessages(List<Message> chunks) =>
            throw new System.NotImplementedException();

        public Message CombineChunkedFileMessages(List<Message> chunks) =>
            throw new System.NotImplementedException();
    }
}
