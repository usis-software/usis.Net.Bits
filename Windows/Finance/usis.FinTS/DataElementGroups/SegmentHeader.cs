//
//  @(#) SegmentHeader.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;
using usis.FinTS.DataElements;

namespace usis.FinTS.DataElementGroups
{
    //  -------------------
    //  SegmentHeader class
    //  -------------------

    internal class SegmentHeader : DataElementGroup
    {
        #region fields

        private AlphanumericDataElement identifier = new AlphanumericDataElement(6, LengthQualifier.Maximum);
        private NumericDataElement segmentNumber = new NumericDataElement(3, LengthQualifier.Maximum);
        private NumericDataElement segmentVersion = new NumericDataElement(3, LengthQualifier.Maximum);
        private NumericDataElement reference = new NumericDataElement(3, LengthQualifier.Maximum);

        #endregion fields

        #region properties

        //  -------------------
        //  Identifier property
        //  -------------------

        internal string Identifier { get => identifier.Value; set => identifier.Value = value; }

        //  ---------------
        //  Number property
        //  ---------------

        internal int Number { set => segmentNumber.Value = value; }

        //  ----------------
        //  Version property
        //  ----------------

        internal int Version { set => segmentVersion.Value = value; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal SegmentHeader()
        {
            AddElements(identifier, segmentNumber, segmentVersion);
            AddElement(reference, true);
        }

        internal SegmentHeader(string segmentIdentifier, int version)
        {
            AddElements(identifier, segmentNumber, segmentVersion);

            Identifier = segmentIdentifier;
            Version = version;
        }

        #endregion construction
    }
}

// eof "SegmentHeader.cs"
