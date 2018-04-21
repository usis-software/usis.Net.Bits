//
//  @(#) IDocRfcFunctionReader.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace usis.Middleware.SAP
{
    //  ---------------------------
    //  IDocRfcFunctionReader class
    //  ---------------------------

    internal sealed class IDocRfcFunctionReader : IDocReader
    {
        #region fields

        private IRfcTable dataRecords;
        private Dictionary<string, IDocControlRecord> controlRecords;
        private int currentIndex = -1;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal IDocRfcFunctionReader(IRfcFunction function) : this(function.GetTable("IDOC_CONTROL_REC_40"), function.GetTable("IDOC_DATA_REC_40")) { }

        private IDocRfcFunctionReader(IRfcTable controlRecordTable, IRfcTable dataRecordTable)
        {
            // keep referenc to data records
            dataRecords = dataRecordTable;

            // build dictionary of control records
            controlRecords = controlRecordTable.Select((r) => new IDocControlRecord(BuildRecordData(r, IDocControlRecord.Definition))).ToDictionary(r => r.DocumentNumber, r => r, StringComparer.Ordinal);
        }

        #endregion construction

        #region IDocReader implementation

        //  -----------
        //  Read method
        //  -----------

        /// <summary>
        /// Reads the next the segment from the reader.
        /// </summary>
        /// <returns>
        /// <c>true</c> if a segment was read; otherwise <c>false</c>.
        /// </returns>

        public override bool Read()
        {
            currentIndex = ++currentIndex < dataRecords.Count ? currentIndex : -1;
            ReadCurrentSegment();

            return currentIndex >= 0;
        }

        #endregion IDocReader implementation

        #region private members

        //  -------------------------
        //  ReadCurrentSegment method
        //  -------------------------

        private void ReadCurrentSegment()
        {
            CurrentDataRecord = GetDataRecord(currentIndex);
            CurrentControlRecord = (CurrentDataRecord != null && controlRecords.TryGetValue(CurrentDataRecord.DocumentNumber, out IDocControlRecord controlRecord)) ? controlRecord : null;
        }

        //  --------------------
        //  GetDataRecord method
        //  --------------------

        private IDocDataRecord GetDataRecord(int index)
        {
            if (index < 0) return null;
            if (index >= dataRecords.RowCount) throw new InvalidOperationException();

            return new IDocDataRecord(BuildRecordData(dataRecords[index], IDocDataRecord.Definition));
        }

        //  ----------------------
        //  BuildRecordData method
        //  ----------------------

        private static string BuildRecordData(IRfcStructure row, IDocRecordDefinition definition)
        {
            var data = new StringBuilder();
            foreach (var field in definition.Fields)
            {
                data.Append(BuildFieldData(row[field.Name], field));
            }
            return data.ToString();
        }

        //  BuildFieldData
        private static string BuildFieldData(IRfcField field, IDocFieldDefinition definition)
        {
            string s = null;
            switch (field.Metadata.DataType)
            {
                case RfcDataType.DATE:
                    s = field.GetString();
                    if (s.Length >= 10) s = s.Replace("-", string.Empty);
                    break;
                case RfcDataType.TIME:
                    s = field.GetString();
                    if (s.Length >= 8) s = s.Replace(":", string.Empty);
                    break;
                case RfcDataType.CHAR:
                case RfcDataType.BYTE:
                case RfcDataType.NUM:
                case RfcDataType.BCD:
                case RfcDataType.UTCLONG:
                case RfcDataType.UTCSECOND:
                case RfcDataType.UTCMINUTE:
                case RfcDataType.DTDAY:
                case RfcDataType.DTWEEK:
                case RfcDataType.DTMONTH:
                case RfcDataType.TSECOND:
                case RfcDataType.TMINUTE:
                case RfcDataType.CDAY:
                case RfcDataType.FLOAT:
                case RfcDataType.INT1:
                case RfcDataType.INT2:
                case RfcDataType.INT4:
                case RfcDataType.INT8:
                case RfcDataType.DECF16:
                case RfcDataType.DECF34:
                case RfcDataType.STRING:
                case RfcDataType.XSTRING:
                case RfcDataType.STRUCTURE:
                case RfcDataType.TABLE:
                case RfcDataType.CLASS:
                case RfcDataType.UNKNOWN:
                default:
                    break;
            }
            return (s ?? field.GetString()).PadRight(definition.Length).Substring(0, definition.Length);
        }

        #endregion private members
    }
}

// eof "IDocRfcFunctionReader.cs"
