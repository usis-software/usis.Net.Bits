//
//  @(#) Enumerations.cs
//
//  Project:    usis Workflow Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

namespace usis.Workflow
{
    #region ProcessDefinitionState enumeration

    //  ----------------------------------
    //  ProcessDefinitionState enumeration
    //  ----------------------------------

    /// <summary>
    /// Specifies the state of a process definition.
    /// </summary>

    public enum ProcessDefinitionState
    {
        /// <summary>
        /// The process definition is disabled.
        /// </summary>

        Disabled,

        /// <summary>
        /// The process definition is enabled.
        /// </summary>

        Enabled
    }

    #endregion ProcessDefinitionState enumeration

    #region ProcessInstanceState enumeration

    //  --------------------------------
    //  ProcessInstanceState enumeration
    //  --------------------------------

    /// <summary>
    /// Specifies the state of a process instance.
    /// </summary>

    public enum ProcessInstanceState
    {
        /// <summary>
        /// The process instance is suspended.
        /// </summary>

        Suspended,

        /// <summary>
        /// The process instance is running.
        /// </summary>

        Running
    }

    #endregion ProcessInstanceState enumeration

    #region ActivityInstanceState enumeration

    //  ---------------------------------
    //  ActivityInstanceState enumeration
    //  ---------------------------------

    /// <summary>
    /// Specifies the state of an activity instance.
    /// </summary>

    public enum ActivityInstanceState
    {
        /// <summary>
        /// The activity instance is suspended.
        /// </summary>

        Suspended,

        /// <summary>
        /// The activity instance is running.
        /// </summary>

        Running
    }

    #endregion ActivityInstanceState enumeration
}

// eof "Enumerations.cs"
