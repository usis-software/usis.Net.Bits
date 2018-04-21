//
//  @(#) WapiEntityId.cs
//
//  Project:    usis Workflow Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Runtime.Serialization;

namespace usis.Workflow
{
    #region WapiEntityId class

    //  ------------------
    //  WapiEntityId class
    //  ------------------

    /// <summary>
    /// Provides a base class for a workflow entity unique identifier.
    /// </summary>

    [DataContract]
    public abstract class WapiEntityId : IEquatable<WapiEntityId>
    {
        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="WapiEntityId"/> class
        /// with the specified GUID.
        /// </summary>
        /// <param name="value">The GUID for the unique identifier's value.</param>

        internal protected WapiEntityId(Guid value) { Value = value; }

        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets the value of the unique identifier..
        /// </summary>
        /// <value>
        /// The value of the unique identifier.
        /// </value>

        [DataMember]
        public Guid Value { get; private set; }

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString() { return Value.ToString(); }

        //  -------------
        //  Equals method
        //  -------------

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>

        public override bool Equals(object obj)
        {
            var id = obj as WapiEntityId;
            if (id == null) return false;

            return Value == id.Value;
        }

        bool IEquatable<WapiEntityId>.Equals(WapiEntityId other)
        {
            return Equals(other);
        }

        //  ------------------
        //  GetHashCode method
        //  ------------------

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>

        public override int GetHashCode() { return Value.GetHashCode(); }
    }

    #endregion WapiEntityId class

    #region ProcessDefinitionId class

    //  -------------------------
    //  ProcessDefinitionId class
    //  -------------------------

    /// <summary>
    /// Defines the unique identifier for a process definition.
    /// </summary>
    /// <seealso cref="WapiEntityId" />

    [DataContract]
    public sealed class ProcessDefinitionId : WapiEntityId
    {
        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessDefinitionId"/> class.
        /// </summary>
        /// <param name="value">The GUID for the unique identifier's value.</param>

        public ProcessDefinitionId(Guid value) : base(value) { }
    }

    #endregion ProcessDefinitionId class

    #region ProcessInstanceId class

    //  -----------------------
    //  ProcessInstanceId class
    //  -----------------------

    /// <summary>
    /// Defines the unique identifier for a process instance.
    /// </summary>
    /// <seealso cref="WapiEntityId" />

    [DataContract]
    public sealed class ProcessInstanceId : WapiEntityId
    {
        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessInstanceId"/> class.
        /// </summary>
        /// <param name="value">The GUID for the unique identifier's value.</param>

        public ProcessInstanceId(Guid value) : base(value) { }
    }

    #endregion ProcessInstanceId class

    #region ActivityInstanceId class

    //  ------------------------
    //  ActivityInstanceId class
    //  ------------------------

    /// <summary>
    /// Defines the unique identifier for an activity instance.
    /// </summary>
    /// <seealso cref="WapiEntityId" />

    [DataContract]
    public sealed class ActivityInstanceId : WapiEntityId
    {
        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityInstanceId"/> class.
        /// </summary>
        /// <param name="value">The GUID for the unique identifier's value.</param>

        public ActivityInstanceId(Guid value) : base(value) { }
    }

    #endregion ActivityInstanceId class
}

// eof "WapiEntityId.cs"
