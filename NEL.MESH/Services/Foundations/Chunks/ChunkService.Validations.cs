using System.IO;
using NEL.MESH.Models.Foundations.Chunking.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;

namespace NEL.MESH.Services.Foundations.Chunks
{
    internal partial class ChunkService
    {
        private static void ValidateMessageIsNotNull(Message message)
        {
            if (message is null)
            {
                throw new NullMessageChunkException(message: "Message chunk is null.");
            }
        }

        private static void ValidateStream(Stream content)
        {
            if (content is null)
            {
                throw new InvalidStreamChunkException(message: "Stream is null.");
            }

            if (!content.CanRead)
            {
                throw new InvalidStreamChunkException(message: "Stream is not readable.");
            }

            if (!content.CanSeek)
            {
                throw new InvalidStreamChunkException(message: "Stream must be seekable to determine chunk count.");
            }
        }
    }
}
