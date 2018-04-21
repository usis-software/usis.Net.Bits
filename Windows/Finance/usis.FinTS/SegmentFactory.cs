//
//  @(#) SegmentFactory.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using usis.FinTS.Segments;

namespace usis.FinTS.Base
{
    //  --------------------
    //  SegmentFactory class
    //  --------------------

    internal static class SegmentFactory
    {
        #region fields

        private static Dictionary<string, Type> dictionary = Initialize();
        private static Dictionary<string, Type> customerDictionary = InitializeCustomer();
        private static Dictionary<string, Type> bankDictionary = InitializeBank();

        #endregion fields

        #region methods

        //  -----------------
        //  Initialize method
        //  -----------------

        private static Dictionary<string, Type> Initialize()
        {
            var dictionary = new Dictionary<string, Type>();

            dictionary.RegisterSegment<MessageHeader>(SegmentIdentifiers.MessageHeader);
            dictionary.RegisterSegment<MessageFooter>(SegmentIdentifiers.MessageFooter);

            dictionary.RegisterSegment<Identification>(SegmentIdentifiers.Identification);
            dictionary.RegisterSegment<ProcessingPreparation>(SegmentIdentifiers.ProcessingPreparation);

            dictionary.RegisterSegment<DialogEnd>(SegmentIdentifiers.DialogEnd);

            return dictionary;
        }

        //  -------------------------
        //  InitializeCustomer method
        //  -------------------------

        private static Dictionary<string, Type> InitializeCustomer()
        {
            var dictionary = new Dictionary<string, Type>();

            dictionary.RegisterSegment<MessageHeader>(SegmentIdentifiers.MessageHeader);

            return dictionary;
        }

        //  ---------------------
        //  InitializeBank method
        //  ---------------------

        private static Dictionary<string, Type> InitializeBank()
        {
            var dictionary = new Dictionary<string, Type>();

            dictionary.RegisterSegment<BankMessageHeader>(SegmentIdentifiers.MessageHeader);
            dictionary.RegisterSegment<MessageFeedback>(SegmentIdentifiers.MessageFeedback);

            return dictionary;
        }

        //  ----------------------
        //  RegisterSegment method
        //  ----------------------

        private static void RegisterSegment<TSegment>(this Dictionary<string, Type> dictionary, string identifier) where TSegment : Segment
        {
            dictionary.Add(identifier, typeof(TSegment));
        }

        //  --------------------
        //  CreateSegment method
        //  --------------------

        private static Segment CreateSegment(string identifier)
        {
            return dictionary.TryGetValue(identifier, out Type type) ?
                Activator.CreateInstance(type) as Segment :
                new UnknownSegment();
        }

        //  ------------------------
        //  CreateBankSegment method
        //  ------------------------

        internal static Segment CreateBankSegment(string identifier)
        {
            return bankDictionary.TryGetValue(identifier, out Type type) ?
                Activator.CreateInstance(type) as Segment :
                CreateSegment(identifier);
        }

        //  ----------------------------
        //  CreateCustomerSegment method
        //  ----------------------------

        internal static Segment CreateCustomerSegment(string identifier)
        {
            return customerDictionary.TryGetValue(identifier, out Type type) ?
                Activator.CreateInstance(type) as Segment :
                CreateSegment(identifier);
        }

        #endregion methods
    }
}

// eof "SegmentFactory.cs"
