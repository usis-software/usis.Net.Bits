//
//  @(#) ProcessingPreparation.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;
using usis.FinTS.DataElements;

namespace usis.FinTS.Segments
{
    //  ---------------------------
    //  ProcessingPreparation class
    //  ---------------------------

    internal sealed class ProcessingPreparation : Segment
    {
        #region fields

        private NumericDataElement bpdVersion = new NumericDataElement(3, LengthQualifier.Maximum);
        private NumericDataElement updVersion = new NumericDataElement(3, LengthQualifier.Maximum);
        private CodeDataElement dialogLanguage = new CodeDataElement(3);
        private AlphanumericDataElement productDescription = new AlphanumericDataElement(25, LengthQualifier.Maximum);
        private AlphanumericDataElement productVersion = new AlphanumericDataElement(5, LengthQualifier.Maximum);

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingPreparation"/> class.
        /// </summary>

        public ProcessingPreparation()
        {
            AddElements(bpdVersion, updVersion, dialogLanguage, productDescription, productVersion);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingPreparation"/> class
        /// with the specified customer system information.
        /// </summary>
        /// <param name="customerSystem">The customer system.</param>

        internal ProcessingPreparation(CustomerSystem customerSystem) : this()
        {
            Identifier = SegmentIdentifiers.ProcessingPreparation;
            Version = 3;

            bpdVersion.Value = customerSystem.BankParameterDataVersion;
            updVersion.Value = customerSystem.UserParameterDataVersion;
            dialogLanguage.Value = customerSystem.DialogLanguage;
            productDescription.Value = customerSystem.ProductDescription;
            productVersion.Value = customerSystem.ProductVersion;
        }

        #endregion construction
    }
}

// eof "ProcessingPreparation.cs"
