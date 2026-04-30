// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Chunks
{
    /// <summary>
    /// A virtual seekable stream that reports a large Length without allocating real memory.
    /// Read operations return zero-filled bytes up to the reported Length.
    /// Used only in tests to verify chunk-count calculations on large files.
    /// </summary>
    internal sealed class LargeVirtualStream : Stream
    {
        private readonly long length;
        private long position;

        public LargeVirtualStream(long length)
        {
            this.length = length;
            this.position = 0;
        }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => this.length;

        public override long Position
        {
            get => this.position;
            set => this.position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long remaining = this.length - this.position;

            if (remaining <= 0)
            {
                return 0;
            }

            int bytesToRead = (int)Math.Min(count, remaining);
            Array.Clear(buffer, offset, bytesToRead);
            this.position += bytesToRead;

            return bytesToRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            this.position = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => this.position + offset,
                SeekOrigin.End => this.length + offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            };

            return this.position;
        }

        public override void Flush() { }
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
