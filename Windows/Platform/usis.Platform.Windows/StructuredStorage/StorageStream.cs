//
//  @(#) StorageStream.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace usis.Platform.StructuredStorage
{
    //  -------------------
    //  StorageStream class
    //  -------------------

    /// <summary>
    /// Provides members to read and write data to streams.
    /// Streams contain the data in a structured storage, where storages provide the structure.
    /// Simple data can be written directly to a stream but,
    /// most frequently, streams are elements nested within a storage object.
    /// They are similar to standard files.
    /// </summary>

    public class StorageStream : Stream
    {
        #region private fields

        private ComTypes.IStream stream;
        private ElementStatistics statistics;

        #endregion private fields

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentNullException">stream</exception>

        internal StorageStream(ComTypes.IStream stream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Releases the unmanaged resources used by the
        /// <see cref="StorageStream"/> and
        /// optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (stream != null)
                {
                    Marshal.ReleaseComObject(stream);
                }
                stream = null;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //  -----------
        //  destruction
        //  -----------

        /// <summary>
        /// This member overrides <see cref="object.Finalize"/>.
        /// </summary>

        ~StorageStream() { Dispose(false); }

        #endregion construction/destruction

        #region public properties

        //  -------------------
        //  Statistics property
        //  -------------------

        /// <summary>
        /// Gets an <see cref="ElementStatistics" /> object that
        /// contains statistical information about the open stream element.
        /// </summary>
        /// <value>
        /// The statistical information about the open stream element.
        /// </value>

        public ElementStatistics Statistics
        {
            get
            {
                if (statistics == null)
                {
                    statistics = new ElementStatistics(this, true);
                }
                return statistics;
            }
        }

        //  ----------------
        //  CanRead property
        //  ----------------

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current stream supports reading; otherwise, <c>false</c>.
        /// </value>

        public override bool CanRead => true;

        //  ----------------
        //  CanSeek property
        //  ----------------

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current stream supports seeking; otherwise, <c>false</c>.
        /// </value>

        public override bool CanSeek => true;

        //  -----------------
        //  CanWrite property
        //  -----------------

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current stream supports writing; otherwise, <c>false</c>.
        /// </value>

        public override bool CanWrite => (Statistics.Mode & StorageModes.ReadWrite) > 0;

        //  ---------------
        //  Length property
        //  ---------------

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        /// <value>
        /// The length in bytes of the stream.
        /// </value>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>

        public override long Length => Stat(STATFLAG.NONAME).cbSize;

        //  -----------------
        //  Position property
        //  -----------------

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        /// <value>
        /// The position within the current stream.
        /// </value>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>

        public override long Position
        {
            get => Seek(0, SeekOrigin.Current);
            set => Seek(value, SeekOrigin.Begin);
        }

        #endregion public properties

        #region private and internal methods

        //  -----------
        //  Stat method
        //  -----------

        private ComTypes.STATSTG Stat(STATFLAG flags)
        {
            Stat(out ComTypes.STATSTG statstg, flags);
            return statstg;
        }

        internal void Stat(out ComTypes.STATSTG statstg, STATFLAG flags)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));
            stream.Stat(out statstg, (int)flags);
        }

        #endregion private and internal methods

        #region public methods

        //  ------------
        //  Flush method
        //  ------------

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data
        /// to be written to the underlying device.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>

        public override void Flush() { Commit(); }

        //  -----------
        //  Read method
        //  -----------

        /// <summary>
        /// Reads a sequence of bytes from the current stream and
        /// advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns,
        /// the buffer contains the specified byte array with the values between
        /// <paramref name="offset" /> and <paramref name="offset" /> + <paramref name="count" /> - 1
        /// replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which
        /// to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer.
        /// This can be less than the number of bytes requested
        /// if that many bytes are not currently available,
        /// or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="buffer" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));

            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (count + offset > buffer.Length)
            {
                throw new ArgumentException(Resources.TextExceptionSumAndOffset, nameof(buffer));
            }
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            int bytes;
            unsafe
            {
                try
                {
                    if (offset == 0)
                    {
                        stream.Read(buffer, count, new IntPtr(&bytes));
                    }
                    else
                    {
                        byte[] data = new byte[count];
                        stream.Read(data, count, new IntPtr(&bytes));
                        Buffer.BlockCopy(data, 0, buffer, offset, bytes);
                    }
                }
                catch (COMException exception)
                {
                    throw new IOException(exception.Message, exception.ErrorCode);
                }
            }
            return bytes;
        }

        //  -----------
        //  Seek method
        //  -----------

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin" /> indicating
        /// the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));

            unsafe
            {
                long newPosition = 0;
                IntPtr pNewPosition = new IntPtr(&newPosition);
                try { stream.Seek(offset, (int)origin, pNewPosition); }
                catch (COMException exception)
                {
                    throw new IOException(exception.Message, exception.ErrorCode);
                }
                return newPosition;
            }
        }

        //  ----------------
        //  SetLength method
        //  ----------------

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>

        public override void SetLength(long value)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));
            if (!CanWrite) throw new NotSupportedException();

            try
            {
                stream.SetSize(value);
                statistics = null;
            }
            catch (COMException exception)
            {
                throw new IOException(exception.Message, exception.ErrorCode);
            }
        }

        //  ------------
        //  Write method
        //  ------------

        /// <summary>
        /// Writes a sequence of bytes to the current stream
        /// and advances the current position within this stream
        /// by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from
        /// <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin
        /// copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
        /// <exception cref="NotImplementedException">Another value that 0 is not supported for the <paramref name="offset"/> parameter.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="buffer" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));
            if (!CanWrite) throw new NotSupportedException();

            if (offset != 0) throw new NotImplementedException();

            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (count > buffer.Length) throw new ArgumentException(Resources.TextExceptionSumAndOffset, nameof(buffer));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            try
            {
                stream.Write(buffer, count, IntPtr.Zero);
                statistics = null;
            }
            catch (COMException exception)
            {
                throw new IOException(exception.Message, exception.ErrorCode);
            }
        }

        //  ------------
        //  Clone method
        //  ------------

        /// <summary>
        /// Creates a new stream object with its own seek pointer
        /// that references the same bytes as the original stream.
        /// </summary>
        /// <returns>
        /// The new stream object.
        /// </returns>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>

        public StorageStream Clone()
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));

            ComTypes.IStream clone;
            try
            {
                stream.Clone(out clone);
            }
            catch (COMException exception)
            {
                throw new IOException(exception.Message, exception.ErrorCode);
            }
            return new StorageStream(clone);
        }

        //  -------------
        //  Commit method
        //  -------------

        /// <summary>
        /// Ensures that any changes made to a stream object
        /// open in transacted mode are reflected in the parent storage.
        /// If the stream object is open in direct mode, <see cref="Commit()" />
        /// has no effect other than flushing all memory buffers
        /// to the next-level storage object.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>

        public void Commit() { Commit(CommitConditions.None); }

        /// <summary>
        /// Ensures that any changes made to a stream object
        /// open in transacted mode are reflected in the parent storage.
        /// If the stream object is open in direct mode, <see cref="Commit(CommitConditions)" />
        /// has no effect other than flushing all memory buffers
        /// to the next-level storage object.
        /// </summary>
        /// <param name="conditions">A value that controls how the changes for the stream object are committed.</param>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>

        public void Commit(CommitConditions conditions)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));

            try
            {
                stream.Commit((int)conditions);
            }
            catch (COMException exception)
            {
                throw new IOException(exception.Message, exception.ErrorCode);
            }
        }

        //  -------------
        //  Revert method
        //  -------------

        /// <summary>
        /// Discards all changes that have been made to a transacted stream
        /// since the last <see cref="Commit(CommitConditions)" /> call.
        /// On streams open in direct mode this method has no effect.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>

        public void Revert()
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));

            try
            {
                stream.Revert();
                statistics = null;
            }
            catch (COMException exception)
            {
                throw new IOException(exception.Message, exception.ErrorCode);
            }
        }

        //  -------------
        //  CopyTo method
        //  -------------

        /// <summary>
        /// Copies a specified number of bytes from the current seek pointer
        /// in the stream to the current seek pointer in another stream.
        /// </summary>
        /// <param name="destination">The destination stream.</param>
        /// <param name="count">The number of bytes to copy from the source stream.</param>
        /// <param name="bytesRead">On successful return,
        /// contains the actual number of bytes read from the source.</param>
        /// <param name="bytesWritten">On successful return,
        /// contains the actual number of bytes written to the destination.</param>
        /// <exception cref="ArgumentNullException"><paramref name="destination" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after at least one of the streams was closed.</exception>

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        public void CopyTo(StorageStream destination, long count, out long bytesRead, out long bytesWritten)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (destination.stream == null) throw new ObjectDisposedException(nameof(destination));

            IntPtr read = IntPtr.Zero;
            IntPtr written = IntPtr.Zero;
            try
            {
                stream.CopyTo(destination.stream, count, read, written);
            }
            catch (COMException exception)
            {
                throw new IOException(exception.Message, exception.ErrorCode);
            }
            bytesRead = read.ToInt64();
            bytesWritten = written.ToInt64();
        }

        #endregion public methods
    }
}

// eof "StorageStream.cs"
