//
//  @(#) IDocSender.cs
//
//  Project:    usis IDoc Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using SAP.Middleware.Connector;
using System;

namespace usis.Middleware.SAP
{
    //  ----------------
    //  IDocSender class
    //  ----------------

    internal static class IDocSender
    {
        //  ---------------
        //  SendFile method
        //  ---------------

        internal static void SendFile(string path, string destinationName)
        {
            using (var reader = new IDocFileReader(path))
            {
                foreach (var document in IDoc.ReadDocuments(reader))
                {
                    var destination = RfcDestinationManager.GetDestination(destinationName);
                    var function = destination.Repository.CreateFunction("IDOC_INBOUND_ASYNCHRONOUS");

                    var controlRecords = function.GetTable("IDOC_CONTROL_REC_40");
                    var dataRecords = function.GetTable("IDOC_DATA_REC_40");

                    controlRecords.Append();
                    controlRecords.CurrentRow.SetValues(document.ControlRecord);

                    foreach (var segment in document.SegmentSequence)
                    {
                        dataRecords.Append();
                        dataRecords.CurrentRow.SetValues(segment.DataRecord);
                    }

                    function.Invoke(destination);

                    Console.WriteLine(Strings.IDocSent, document);
                }
            }
        }
    }

    #region Extensions class

    //  ----------------
    //  Extensions class
    //  ----------------

    internal static class Extensions
    {
        //  ----------------
        //  SetValues method
        //  ----------------

        internal static void SetValues(this IRfcStructure row, IDocRecord record)
        {
            foreach (var field in record.Values)
            {
                row.SetValue(field.Name, field.Value);
            }
        }
    }

    #endregion Extensions class
}

// eof "IDocSender.cs"
