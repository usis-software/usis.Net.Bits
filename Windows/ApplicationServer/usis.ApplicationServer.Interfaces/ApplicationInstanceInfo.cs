//
//  @(#) ApplicationInstanceInfo.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace usis.ApplicationServer
{
    #region ApplicationInstanceState enumeration

    //  ------------------------------------
    //  ApplicationInstanceState enumeration
    //  ------------------------------------

    /// <summary>
    /// Indicates the current state of an application instance.
    /// </summary>

    public enum ApplicationInstanceState
    {
        /// <summary>
        /// The application is disabled.
        /// </summary>

        Disabled,

        /// <summary>
        /// The application is stopped.
        /// </summary>

        Stopped,

        /// <summary>
        /// The application is starting.
        /// </summary>

        Starting,

        /// <summary>
        /// The application is running.
        /// </summary>

        Running,

        /// <summary>
        /// The application pause is pending.
        /// </summary>

        Pausing,

        /// <summary>
        /// The application is paused.
        /// </summary>

        Paused,

        /// <summary>
        /// The application resume is pending.
        /// </summary>

        Resuming,

        /// <summary>
        /// The application is stopping.
        /// </summary>

        Stopping,

        /// <summary>
        /// The application failed during startup.
        /// </summary>

        Failed
    }

    #endregion ApplicationInstanceState enumeration

    //  -----------------------------
    //  ApplicationInstanceInfo class
    //  -----------------------------

    /// <summary>
    /// Provides informations about an instance of an application
    /// </summary>

    [DataContract(Name = "Application")]
    public class ApplicationInstanceInfo
    {
        #region properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>

        [DataMember]
        public string Name { get; set; }

        //  -----------
        //  Id property
        //  -----------

        /// <summary>
        /// Gets or sets the instance identifier for the application.
        /// </summary>
        /// <value>
        /// The instance identifier for the application.
        /// </value>

        [DataMember]
        public Guid Id { get; set; }

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets or sets a value that indicates the current state of an application instance.
        /// </summary>
        /// <value>
        /// A value that indicates the current state of an application instance.
        /// </value>

        [DataMember]
        public ApplicationInstanceState State { get; set; }

        #endregion properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture,
                "Application instance '{0}'; Id={1}; Status={2}",
                Name, Id.ToString(), State.ToString());
        }

        #endregion overrides
    }
}

// eof "ApplicationInstanceInfo.cs"
