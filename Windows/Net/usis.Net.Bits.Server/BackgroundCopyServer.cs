//
//  @(#) BackgroundCopyServer.cs
//
//  Project:    usis.Net.Bits.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using usis.Platform.Net;
using usis.Platform;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace usis.Net.Bits
{
    //  --------------------------
    //  BackgroundCopyServer class
    //  --------------------------

    /// <summary>
    /// Implements a HTTP server that supports BITS
    /// downloads (HTTP/1.1 Ranges Requests) and
    /// uploads (BITS Upload Protocol).
    /// </summary>
    /// <seealso cref="HttpServer" />

    public abstract class BackgroundCopyServer : HttpServer
    {
        #region overrides

        //  --------------
        //  Process method
        //  --------------

        /// <summary>
        /// Processes a request that is specified by the provided context.
        /// </summary>
        /// <param name="context">The context of the HTTP request.</param>
        /// <remarks>
        /// Do not override this method without calling the base class implementation because it handles all BITS requests.
        /// </remarks>

        protected override void Process(HttpListenerContext context)
        {
            ProcessBitsRequest(context);
        }

        #endregion overrides

        #region virtual methods

        //  --------------------
        //  StartDownload method
        //  --------------------

        /// <summary>
        /// This method is called when a HTTP HEAD request is processed.
        /// </summary>
        /// <param name="url">The URL of the download request, including query string.</param>
        /// <returns>
        ///   <c>true</c> if the URL is valid and a download stream can by accossiated with the request.
        /// </returns>

        protected virtual bool StartDownload(Uri url) { return false; }

        //  ------------------------
        //  GetDownloadStream method
        //  ------------------------

        /// <summary>
        /// Gets the stream for a download request.
        /// </summary>
        /// <param name="url">The URL of the download request, including query string.</param>
        /// <returns>
        /// The stream that was created to download the response data.
        /// </returns>

        protected virtual Stream GetDownloadStream(Uri url) { return null; }

        //  --------------------------
        //  CreateUploadSession method
        //  --------------------------

        /// <summary>
        /// This method is called to respond to a <c>Create-Session</c> request.
        /// </summary>
        /// <param name="url">The URL of the upload request, including query string.</param>
        /// <returns>A unique identifier for the upload session.</returns>

        protected virtual string CreateUploadSession(Uri url) { return null; }

        //  ----------------------
        //  GetUploadStream method
        //  ----------------------

        /// <summary>
        /// Gets the stream for a upload session.
        /// </summary>
        /// <param name="sessionId">The unique session identifier.</param>
        /// <returns>
        /// The stream that was created to upload the session data.
        /// </returns>

        protected virtual Stream GetUploadStream(string sessionId) { return null; }

        //  -------------------------
        //  CloseUploadSession method
        //  -------------------------

        /// <summary>
        /// This method is called to respond to a <c>Close-Session</c> request.
        /// </summary>
        /// <param name="sessionId">The unique session identifier.</param>

        protected virtual void CloseUploadSession(string sessionId) { }

        //  --------------------------
        //  CancelUploadSession method
        //  --------------------------

        /// <summary>
        /// This method is called to respond to a <c>Cancel-Session</c> request.
        /// </summary>
        /// <param name="sessionId">The unique session identifier.</param>

        protected virtual void CancelUploadSession(string sessionId) { }

        //  ------------------------
        //  GetUploadReplyUrl method
        //  ------------------------

        /// <summary>
        /// This method is called during an upload session when the last fragment was uploaded
        /// to add a reply URL to the response header.
        /// </summary>
        /// <param name="sessionId">The unique session identifier.</param>
        /// <returns>The reply URL to add to the response header.</returns>

        protected virtual Uri GetUploadReplyUrl(string sessionId) { return null; }

        #endregion virtual methods

        #region private processing methods

        //  -------------------------
        //  ProcessBitsRequest method
        //  -------------------------

        private void ProcessBitsRequest(HttpListenerContext context)
        {
            const StringComparison methodComparison = StringComparison.Ordinal;

            if (HttpMethod.Get.Method.Equals(context.Request.HttpMethod, methodComparison))
            {
                ProcessBitsGet(context); // GET
            }
            else if (HttpMethod.Head.Method.Equals(context.Request.HttpMethod, methodComparison))
            {
                ProcessBitsHead(context); // HEAD
            }
            else if (Constants.BitsPostMethodName.Equals(context.Request.HttpMethod, methodComparison))
            {
                const StringComparison headerComparision = StringComparison.OrdinalIgnoreCase;

                var packetType = context.Request.Headers.Get(UploadHeaderNames.PacketType);
                if (UploadPacketType.Fragment.Equals(packetType, headerComparision))
                {
                    ProcessBitsFragment(context); // Fragment
                }
                else if (UploadPacketType.CreateSession.Equals(packetType, headerComparision))
                {
                    ProcessBitsCreateSession(context); // Create-Session
                }
                else if (UploadPacketType.CloseSession.Equals(packetType, headerComparision))
                {
                    ProcessBitsCloseSession(context); // Close-Session
                }
                else if (UploadPacketType.CancelSession.Equals(packetType, headerComparision))
                {
                    ProcessBitsCancelSession(context); // Cancel-Session
                }
                else if (UploadPacketType.Ping.Equals(packetType, headerComparision))
                {
                    ProcessBitsPing(context); // Ping 
                }
                else context.Response.SetStatus(HttpStatusCode.BadRequest);
            }
            else context.Response.SetStatus(HttpStatusCode.MethodNotAllowed);

            context.Response.Close();
        }

        //  ----------------------
        //  ProcessBitsHead method
        //  ----------------------

        private void ProcessBitsHead(HttpListenerContext context)
        {
            // offer a way to check the download
            if (StartDownload(context.Request.Url))
            {
                // accept range request
                context.Response.Headers.Add(HttpResponseHeader.AcceptRanges, Constants.RangeUnit);
                using (var source = GetDownloadStreamFromUrl(context.Request.Url))
                {
                    context.Response.ContentLength64 = source.Length;
                }
                context.Response.SetStatus(HttpStatusCode.OK);
            }
            else context.Response.SetStatus(HttpStatusCode.NotFound);
        }

        //  ---------------------
        //  ProcessBitsGet method
        //  ---------------------

        private void ProcessBitsGet(HttpListenerContext context)
        {
            using (var stream = GetDownloadStreamFromUrl(context.Request.Url))
            {
                if (RangeHeaderValue.TryParse(context.Request.Headers.Get(HttpRequestHeaderName.Range), out RangeHeaderValue rangeValues))
                {
                    if (Constants.RangeUnit.Equals(rangeValues.Unit, StringComparison.OrdinalIgnoreCase) && rangeValues.Ranges.Count > 0)
                    {
                        var rangeItem = rangeValues.Ranges.First();
                        if (rangeItem.From.HasValue && rangeItem.To.HasValue && rangeItem.To.Value >= rangeItem.From.Value)
                        {
                            var from = rangeItem.From.Value;
                            var to = Math.Min(rangeItem.To.Value, stream.Length - 1);

                            var buffer = new byte[to - from + 1];
                            if (stream.Seek(from, SeekOrigin.Begin) == from)
                            {
                                if (stream.Read(buffer, 0, buffer.Length) == buffer.Length)
                                {
                                    context.Response.Headers.Add(
                                        HttpResponseHeader.ContentRange,
                                        new ContentRangeHeaderValue(from, to, stream.Length).ToString());
                                    context.Response.ContentLength64 = buffer.Length;
                                    context.Response.SetStatus(HttpStatusCode.PartialContent);
                                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                                    context.Response.OutputStream.Flush();
                                    context.Response.OutputStream.Close();
                                    return;
                                }
                            }
                            context.Response.SetStatus(HttpStatusCode.RequestedRangeNotSatisfiable);
                        }
                    }
                    context.Response.SetStatus(HttpStatusCode.BadRequest);
                }
                else
                {
                    context.Response.SetStatus(HttpStatusCode.OK);
                    context.Response.ContentLength64 = stream.Length;
                    stream.CopyTo(context.Response.OutputStream);
                }
            }
        }

        //  ----------------------
        //  ProcessBitsPing method
        //  ----------------------

        private static void ProcessBitsPing(HttpListenerContext context)
        {
            // create response to Ping packet
            context.Response.AddHeader(UploadHeaderNames.PacketType, UploadPacketType.Acknowledge);
            context.Response.SetStatus(HttpStatusCode.OK);
        }

        //  -------------------------------
        //  ProcessBitsCreateSession method
        //  -------------------------------

        private void ProcessBitsCreateSession(HttpListenerContext context)
        {
            // check requested protocols
            var protocolId = ChooseProtocol(context.Request.Headers.Get(UploadHeaderNames.SupportedProtocols)?.Trim());

            // create sessionId
            var sessionId = CreateUploadSession(context.Request.Url) ?? Guid.NewGuid().ToString("B");

            // provide response packet
            context.Response.AddHeader(UploadHeaderNames.PacketType, UploadPacketType.Acknowledge);
            context.Response.AddHeader(UploadHeaderNames.Protocol, protocolId.ToString("B"));
            context.Response.AddHeader(UploadHeaderNames.SessionId, sessionId);
            context.Response.AddHeader(HttpResponseHeaderName.AcceptEncoding, Constants.EncodingIdentity);

            context.Response.SetStatus(HttpStatusCode.OK);
        }

        //  --------------------------
        //  ProcessBitsFragment method
        //  --------------------------

        private void ProcessBitsFragment(HttpListenerContext context)
        {
            // set response packet type
            context.Response.AddHeader(UploadHeaderNames.PacketType, UploadPacketType.Acknowledge);

            // get session identifier and set it in the response packet
            var sessionId = context.Request.Headers.Get(UploadHeaderNames.SessionId);
            context.Response.AddHeader(UploadHeaderNames.SessionId, sessionId);

            using (var stream = GetSessionStreamFromSessionId(sessionId))
            {
                var status = HttpStatusCode.OK;

                if (ContentRangeHeaderValue.TryParse(
                    context.Request.Headers.Get(HttpRequestHeaderName.ContentRange), out ContentRangeHeaderValue range)
                    && Constants.RangeUnit.Equals(range.Unit, StringComparison.OrdinalIgnoreCase)
                    && range.HasRange)
                {
                    var offset = Stream.Null.Equals(stream) ? range.To.Value + 1 : stream.Length;
                    var length = range.To.Value - offset + 1;
                    var skip = offset - range.From.Value;
                    if (length > 0)
                    {
                        if (skip > 0)
                        {
                            var buffer = new byte[skip];
                            context.Request.InputStream.Read(buffer, 0, buffer.Length);
                        }
                        if (skip >= 0)
                        {
                            context.Request.InputStream.CopyTo(stream, Convert.ToInt32(length));
                            offset = range.To.Value + 1;
                        }
                        else status = HttpStatusCode.RequestedRangeNotSatisfiable;
                    }
                    context.Response.AddHeader(UploadHeaderNames.ReceivedContentRange, offset.ToString(CultureInfo.InvariantCulture));
                    if (range.HasLength && range.To.Value == range.Length - 1)
                    {
                        var reply = GetUploadReplyUrl(sessionId);
                        if (reply != null) context.Response.Headers.Add(UploadHeaderNames.ReplyUrl, reply.ToString());
                    }
                }
                else status = HttpStatusCode.BadRequest;

                context.Response.ContentLength64 = 0;
                context.Response.SetStatus(status);
            }
        }

        //  ------------------------------
        //  ProcessBitsCloseSession method
        //  ------------------------------

        private void ProcessBitsCloseSession(HttpListenerContext context)
        {
            var sessionId = context.Request.Headers.Get(UploadHeaderNames.SessionId);
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                context.Response.SetStatus(HttpStatusCode.BadRequest);
            }
            else
            {
                CloseUploadSession(sessionId);
                context.Response.AddHeader(UploadHeaderNames.PacketType, UploadPacketType.Acknowledge);
                context.Response.AddHeader(UploadHeaderNames.SessionId, sessionId);
                context.Response.SetStatus(HttpStatusCode.OK);
            }
        }

        //  -------------------------------
        //  ProcessBitsCancelSession method
        //  -------------------------------

        private void ProcessBitsCancelSession(HttpListenerContext context)
        {
            var sessionId = context.Request.Headers.Get(UploadHeaderNames.SessionId);
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                context.Response.SetStatus(HttpStatusCode.BadRequest);
            }
            else
            {
                CancelUploadSession(sessionId);
                context.Response.AddHeader(UploadHeaderNames.PacketType, UploadPacketType.Acknowledge);
                context.Response.AddHeader(UploadHeaderNames.SessionId, sessionId);
                context.Response.SetStatus(HttpStatusCode.OK);
            }
        }

        #endregion private processing methods

        #region private methods

        //  -------------------------------
        //  GetDownloadStreamFromUrl method
        //  -------------------------------

        private Stream GetDownloadStreamFromUrl(Uri url)
        {
            return GetDownloadStream(url) ?? Stream.Null;
        }

        //  ------------------------------------
        //  GetSessionStreamFromSessionId method
        //  ------------------------------------

        private Stream GetSessionStreamFromSessionId(string sessionId)
        {
            return GetUploadStream(sessionId) ?? Stream.Null;
        }

        //  ---------------------
        //  ChooseProtocol method
        //  ---------------------

        private static Guid ChooseProtocol(string supportedProtocols)
        {
            if (string.IsNullOrEmpty(supportedProtocols)) throw new BackgroundCopyServerException(Strings.NoProtcol);

            return ChooseProtocol(supportedProtocols.Split(CharConstants.Space).Select(s => new Guid(s)).ToArray());
        }

        private static Guid ChooseProtocol(params Guid[] supportedProtocols)
        {
            var protocols = new HashSet<Guid>(supportedProtocols);
            if (!protocols.Contains(UploadProtocol.Bits_1_5))
            {
                throw new BackgroundCopyServerException(Strings.WrongProtocol);
            }
            return UploadProtocol.Bits_1_5;
        }

        #endregion private methods
    }

    #region BackgroundCopyServerException class

    //  -----------------------------------
    //  BackgroundCopyServerException class
    //  -----------------------------------

    /// <summary>
    /// Represents errors that occur during processing of requests in the BITS server.
    /// </summary>
    /// <seealso cref="Exception" />

    [Serializable]
    public class BackgroundCopyServerException : Exception
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyServerException"/> class.
        /// </summary>

        public BackgroundCopyServerException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyServerException"/> class
        /// with the specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>

        public BackgroundCopyServerException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyServerException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception,
        /// or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.
        /// </param>

        public BackgroundCopyServerException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyServerException" /> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>

        protected BackgroundCopyServerException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion construction
    }

    #endregion BackgroundCopyServerException class
}

// eof "BackgroundCopyServer.cs"
