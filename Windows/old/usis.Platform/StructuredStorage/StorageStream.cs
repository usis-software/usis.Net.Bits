//
//  @(#) StorageStream.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

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
    /// Lets you read and write data to stream objects.
    /// Stream objects contain the data in a structured storage object,
    /// where storages provide the structure.
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

        //  ------------------------
        //  construction/destruction
        //  ------------------------
        
        internal StorageStream(ComTypes.IStream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            this.stream = stream;
        }

        /// <summary>
        /// This member overrides <see cref="object.Finalize"/>.
        /// </summary>

        ~StorageStream()
        {
            Dispose(false);
        }

        #endregion construction/destruction

        #region public properties

        //  -------------------
        //  Statistics property
        //  -------------------

        /// <summary>
        /// Get an <see cref="ElementStatistics"/> object that
        /// contains statistical information about the open stream element.
        /// </summary>

        public ElementStatistics Statistics
        {
            get
            {
                if (statistics == null)
                {
                    statistics = new ElementStatistics(stream, true);
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
        
        public override bool CanRead
        {
            get { return true; }
        }

        //  ----------------
        //  CanSeek property
        //  ----------------

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        
        public override bool CanSeek
        {
            get { return true; }
        }

        //  -----------------
        //  CanWrite property
        //  -----------------

        /// <summary>
        /// gets a value indicating whether the current stream supports writing.
        /// </summary>
        
        public override bool CanWrite
        {
            get { return (Statistics.Mode & StorageModes.ReadWrite) > 0; }
        }

        //  ---------------
        //  Length property
        //  ---------------

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>

        public override long Length
        {
            get
            {
                RefreshStatistics();
                return Statistics.Size;
            }
        }

        //  -----------------
        //  Position property
        //  -----------------

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>

        public override long Position
        {
            get { return Seek(0, SeekOrigin.Current); }
            set { Seek(value, SeekOrigin.Begin); }
        }

        #endregion public properties

        #region private methods

        //  ------------------------
        //  RefreshStatistics method
        //  ------------------------
        
        private void RefreshStatistics()
        {
            if (statistics != null)
            {
                statistics.Refresh(stream, true);
            }
        }

        #endregion private methods

        #region protected methods

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Releases the unmanaged resources used by the
        /// <see cref="StorageStream"/> and
        /// optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.
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

        #endregion protected methods

        #region public methods

        //  ------------
        //  Flush method
        //  ------------

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data
        /// to be written to the underlying device.
        /// </summary>
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>
        
        public override void Flush()
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));
            try { stream.Commit(0); }
            catch (COMException exception)
            {
                throw new IOException(exception.Message, exception.ErrorCode);
            }
        }

        //  -----------
        //  Read method
        //  -----------

        /// <summary>
        /// Reads a sequence of bytes from the current stream and
        /// advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes. When this method returns,
        /// the buffer contains the specified byte array with the values between
        /// <i>offset</i> and (<i>offset</i> + <i>count</i> - 1)
        /// replaced by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in <i>buffer</i> at which
        /// to begin storing the data read from the current stream.
        /// </param>
        /// <param name="count">
        /// The maximum number of bytes to be read from the current stream.
        /// </param>
        /// <returns>
        /// The total number of bytes read into the buffer.
        /// This can be less than the number of bytes requested
        /// if that many bytes are not currently available,
        /// or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <i>buffer</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The sum of offset and count is larger than the buffer length.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <i>offset</i> or <i>count</i> is negative.
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>
        
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
        /// <param name="offset">
        /// A byte offset relative to the <i>origin</i> parameter.
        /// </param>
        /// <param name="origin">
        /// A value of type <see cref="SeekOrigin"/> indicating
        /// the reference point used to obtain the new position.
        /// </param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>

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
        /// <param name="value">
        /// The desired length of the current stream in bytes.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// The stream does not support writing.
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>
        
        public override void SetLength(long value)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));
            if (!CanWrite) throw new NotSupportedException();

            try { stream.SetSize(value); }
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
        /// <param name="buffer">
        /// An array of bytes. This method copies <i>count</i> bytes from
        /// <i>buffer</i> to the current stream.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in <i>buffer</i> at which to begin
        /// copying bytes to the current stream.
        /// </param>
        /// <param name="count">
        /// The number of bytes to be written to the current stream.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>buffer</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The sum of offset and count is larger than the buffer length.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <i>offset</i> or <i>count</i> is negative.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The stream does not support writing.
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));
            if (!CanWrite) throw new NotSupportedException();

            if (offset != 0) throw new NotImplementedException();

            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (count > buffer.Length) throw new ArgumentException(Resources.TextExceptionSumAndOffset, nameof(buffer));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            try { stream.Write(buffer, count, IntPtr.Zero); }
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
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>
        
        public StorageStream Clone()
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));

            ComTypes.IStream clone;
            try { stream.Clone(out clone); }
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
        /// If the stream object is open in direct mode, <b>Commit</b>
        /// has no effect other than flushing all memory buffers
        /// to the next-level storage object.
        /// </summary>
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>
        
        public void Commit()
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));

            try { stream.Commit((int)CommitConditions.None); }
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
        /// since the last <see cref="Commit"/> call.
        /// On streams open in direct mode this method has no effect.
        /// </summary>
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>
        
        public void Revert()
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));

            try { stream.Revert(); }
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
        /// <param name="destination">
        /// The destination stream.
        /// </param>
        /// <param name="count">
        /// The number of bytes to copy from the source stream.
        /// </param>
        /// <param name="bytesRead">
        /// On successful return,
        /// contains the actual number of bytes read from the source.
        /// </param>
        /// <param name="bytesWritten">
        /// On successful return,
        /// contains the actual number of bytes written to the destination.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>destination</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        public void CopyTo(
            StorageStream destination,
            long count,
            out long bytesRead,
            out long bytesWritten)
        {
            if (stream == null) throw new ObjectDisposedException(nameof(StorageStream));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (destination.stream == null) throw new ObjectDisposedException(nameof(destination));

            IntPtr read = IntPtr.Zero;
            IntPtr written = IntPtr.Zero;
            try { stream.CopyTo(destination.stream, count, read, written); }
            catch (COMException exception)
            {
                throw new IOException(exception.Message, exception.ErrorCode);
            }
            bytesRead = read.ToInt64();
            bytesWritten = read.ToInt64();
        }

        #endregion public methods
    }
}

// eof "StorageStream.cs"
