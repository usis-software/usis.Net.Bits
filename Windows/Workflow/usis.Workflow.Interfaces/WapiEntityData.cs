//
//  @(#) WapiEntityData.cs
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
    #region IWapiEntityData interface

    //  -------------------------
    //  IWapiEntityData interface
    //  -------------------------

    /// <summary>
    /// Provides the definition for a workflow API data entity.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>

    public interface IWapiEntityData<TId>
    {
        //  -----------
        //  Id property
        //  -----------

        /// <summary>
        /// Gets or sets the unique identifier of a data entity.
        /// </summary>
        /// <value>
        /// The unique identifier of a data entity.
        /// </value>

        TId Id { get; set; }
    }

    #endregion IWapiEntityData interface

    #region ProcessDefinitionData class

    //  ---------------------------
    //  ProcessDefinitionData class
    //  ---------------------------

    /// <summary>
    /// Provides the properties for a process definition.
    /// </summary>
    /// <seealso cref="IWapiEntityData{ProcessDefinitionId}" />

    [DataContract]
    public sealed class ProcessDefinitionData : IWapiEntityData<ProcessDefinitionId>
    {
        //  -----------
        //  Id property
        //  -----------

        /// <summary>
        /// Gets or sets the unique identifier of the process definition.
        /// </summary>
        /// <value>
        /// The unique identifier of the process definition.
        /// </value>

        [DataMember]
        public ProcessDefinitionId Id { get; set; }

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets or sets the name of the process definition.
        /// </summary>
        /// <value>
        /// The name of the process definition.
        /// </value>

        [DataMember]
        public string Name { get; set; }

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets or sets the state of the process definition.
        /// </summary>
        /// <value>
        /// The state of the process definition.
        /// </value>

        [DataMember]
        public ProcessDefinitionState State { get; set; }

        //  --------------------
        //  Description property
        //  --------------------

        /// <summary>
        /// Gets or sets the description of the process definition.
        /// </summary>
        /// <value>
        /// The description of the process definition.
        /// </value>

        [DataMember]
        public string Description { get; set; }

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets or sets the date and time when the process definition was created.
        /// </summary>
        /// <value>
        /// The date and time when the process definition was created.
        /// </value>

        [DataMember]
        public DateTime Created { get; set; }

        //  ----------------
        //  Changed property
        //  ----------------

        /// <summary>
        /// Gets or sets the date and time when the process definition was last changed.
        /// </summary>
        /// <value>
        /// The date and time when the process definition was last changed.
        /// </value>

        [DataMember]
        public DateTime? Changed { get; set; }
    }

    #endregion ProcessDefinitionData class

    #region ProcessInstanceData class

    //  -------------------------
    //  ProcessInstanceData class
    //  -------------------------

    /// <summary>
    /// Provides the properties for a process instance.
    /// </summary>
    /// <seealso cref="IWapiEntityData{ProcessInstanceId}" />

    [DataContract]
    public sealed class ProcessInstanceData : IWapiEntityData<ProcessInstanceId>
    {
        //  -----------
        //  Id property
        //  -----------

        /// <summary>
        /// Gets or sets the unique identifier of the process instance.
        /// </summary>
        /// <value>
        /// The unique identifier of the process instance.
        /// </value>

        [DataMember]
        public ProcessInstanceId Id { get; set; }

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets or sets the state of the process instance.
        /// </summary>
        /// <value>
        /// The state of the process instance.
        /// </value>

        [DataMember]
        public ProcessInstanceState State { get; set; }
    }

    #endregion ProcessInstanceData class

    #region ActivityInstanceData class

    //  --------------------------
    //  ActivityInstanceData class
    //  --------------------------

    /// <summary>
    /// Provides the properties for an activity instance.
    /// </summary>
    /// <seealso cref="IWapiEntityData{ActivityInstanceId}" />

    [DataContract]
    public sealed class ActivityInstanceData : IWapiEntityData<ActivityInstanceId>
    {
        //  -----------
        //  Id property
        //  -----------

        /// <summary>
        /// Gets or sets the unique identifier of the activity instance.
        /// </summary>
        /// <value>
        /// The unique identifier of the activity instance.
        /// </value>

        [DataMember]
        public ActivityInstanceId Id { get; set; }

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets or sets the state of the activity instance.
        /// </summary>
        /// <value>
        /// The state of the activity instance.
        /// </value>

        [DataMember]
        public ActivityInstanceState State { get; set; }
    }

    #endregion ActivityInstanceData class
}

// eof "WapiEntityData.cs"
