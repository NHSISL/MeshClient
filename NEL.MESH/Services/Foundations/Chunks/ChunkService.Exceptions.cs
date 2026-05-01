// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using NEL.MESH.Models.Foundations.Chunking.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using Xeptions;

namespace NEL.MESH.Services.Foundations.Chunks
{
    internal partial class ChunkService
    {
        private delegate List<Message> RetruningMessageListFunction();
        private delegate IEnumerable<(Message Message, byte[] Content)> ReturningMessageEnumerableFunction();

        private List<Message> TryCatch(RetruningMessageListFunction retruningMessageListFunction)
        {
            try
            {
                return retruningMessageListFunction();
            }
            catch (NullMessageChunkException nullMessageChunkException)
            {
                throw CreateValidationException(nullMessageChunkException);
            }
            catch (Exception exception)
            {
                var failedChunkServiceException =
                    new FailedChunkServiceException(
                        message: "Chunk service error occurred, contact support.",
                        innerException: exception);

                throw CreateServiceException(failedChunkServiceException);
            }
        }

        private IEnumerable<(Message Message, byte[] Content)> TryCatch(
            ReturningMessageEnumerableFunction returningMessageEnumerableFunction)
        {
            try
            {
                return returningMessageEnumerableFunction();
            }
            catch (NullMessageChunkException nullMessageChunkException)
            {
                throw CreateValidationException(nullMessageChunkException);
            }
            catch (InvalidStreamChunkException invalidStreamChunkException)
            {
                throw CreateValidationException(invalidStreamChunkException);
            }
            catch (Exception exception)
            {
                var failedChunkServiceException =
                    new FailedChunkServiceException(
                        message: "Chunk service error occurred, contact support.",
                        innerException: exception);

                throw CreateServiceException(failedChunkServiceException);
            }
        }

        private ChunkValidationException CreateValidationException(Xeption exception)
        {
            var chunkValidationException = new ChunkValidationException(
                message: "Chunk validation errors occurred, please try again.",
                innerException: exception);

            return chunkValidationException;
        }

        private ChunkServiceException CreateServiceException(Xeption exception)
        {
            var chunkServiceException = new ChunkServiceException(
                message: "Chunk service error occurred, contact support.",
                innerException: exception);

            return chunkServiceException;
        }
    }
}
