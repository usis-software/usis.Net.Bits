//
//  @(#) FoundationExtensions.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using Newtonsoft.Json.Linq;
using System;

namespace usis.Cocoa.Foundation
{
    //  --------------------------
    //  FoundationExtensions class
    //  --------------------------

    /// <summary>
    /// Provides extension methods to <b>Foundation</b> classes.
    /// </summary>

    public static class FoundationExtensions
    {
        #region ToJObject

        //  ----------------
        //  ToJObject method
        //  ----------------

        /// <summary>
        /// Converts the dictionary to a JSON object.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>A JSON object.</returns>
        /// <exception cref="ArgumentNullException"><b>dictionary</b> is a null reference.</exception>

        public static JObject ToJObject(this NSDictionary dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            var obj = new JObject();
            foreach (var key in dictionary.Keys)
            {
                var propertyName = key.Description;
                var value = dictionary.ObjectForKey(key).ToJToken();
                obj.Add(propertyName, value);
            }
            return obj;
        }

        #endregion ToJObject

        #region ToJToken method

        //  ---------------
        //  ToJToken method
        //  ---------------

        /// <summary>
        /// Converts the <b>Foundation</b> object to a JSON token.
        /// </summary>
        /// <param name="obj">The <b>Foundation</b> object.</param>
        /// <returns>
        /// A JSON token.
        /// </returns>
        /// <exception cref="ArgumentNullException"><b>obj</b> is a null reference.</exception>

        private static JToken ToJToken(this NSObject obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (obj is NSDictionary dictionary)
            {
                return dictionary.ToJObject();
            }
            else return JValue.CreateString(obj.Description);
        }

        #endregion ToJToken method
    }
}

// eof "FoundationExtensions.cs"
