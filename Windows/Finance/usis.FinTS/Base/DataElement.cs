//
//  @(#) DataElement.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace usis.FinTS.Base
{
    //  -----------------
    //  DataElement class
    //  -----------------

    internal abstract class DataElement : ISegmentElement
    {
        #region fields

        private string data = string.Empty;

        #endregion fields

        #region properties

        //  ------------------------
        //  DataElementType property
        //  ------------------------

        internal protected DataElementType DataElementType { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal protected DataElement(DataElementType dataElementType) { DataElementType = dataElementType; }

        #endregion construction

        #region methods

        //  ---------------
        //  GetValue method
        //  ---------------

        internal protected string GetValue() { return data; }

        //  ---------------
        //  SetValue method
        //  ---------------

        internal protected void SetValue(string value) { data = Validate(value); }

        //  ---------------
        //  Validate method
        //  ---------------

        internal protected virtual string Validate(string value) { return DataElementType.Validate(value); }

        #region ISegmentElement implementation

        //  ----------------
        //  Serialize method
        //  ----------------

        void ISegmentElement.Serialize(StreamWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            var s = data;
            if (DataElementType.BaseDataType == BaseDataType.Alphanumeric ||
                DataElementType.BaseDataType == BaseDataType.Text)
            {
                // escape characters

                s = data
                    .Replace(Constants.Escape, Constants.EscapedEscape)
                    .Replace(Constants.DataElementSeparator, Constants.EscapedDataElementSeparator)
                    .Replace(Constants.DataElementGroupSeparator, Constants.EscapedDataElementGroupSeparator)
                    .Replace(Constants.SegmentEnd, Constants.EscapedSegmentEnd);
            }
            writer.Write(s);
        }

        //  ------------------
        //  Deserialize method
        //  ------------------

        bool ISegmentElement.Deserialize(StreamReader reader, char terminator, bool optional)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var sb = new StringBuilder();
            var a = new char[1];
            var escaped = false;
            while (reader.Read(a, 0, 1) != 0)
            {
                var c = a[0];
                if (c.IsSeparatorCharacter() && !escaped)
                {
                    SetValue(sb.ToString());

                    // TODO: check for optional element
                    //System.Diagnostics.Debug.Assert(c == terminator);

                    if (terminator == Constants.DataElementGroupSeparatorCharacter &&
                        c == Constants.DataElementSeparatorCharacter)
                    {
                        // data element group terminated with missing optional elements by next data element.
                        return false;
                    }

                    return true;
                }
                escaped = (c == Constants.EscapeCharacter) && !escaped;
                if (!escaped) sb.Append(c);
            }
            throw new InvalidOperationException();
        }

        #endregion ISegmentElement implementation

        #endregion methods

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}: Value={1}", DataElementType, data);
        }

        #endregion overrides
    }
}

// eof "DataElement.cs"
