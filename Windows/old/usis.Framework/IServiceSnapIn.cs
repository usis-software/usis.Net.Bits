//
//  @(#) IServiceSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

namespace usis.Framework
{
    //  ------------------------
    //  IServiceSnapIn interface
    //  ------------------------

    /// <summary>
    /// The <b>IServiceSnapIn</b> interface allow an implementing class to
    /// pause and resume operations when the hosting application
    /// pauses or continues.
    /// </summary>

    public interface IServiceSnapIn
    {
        //  -------------------
        //  PauseOperate method
        //  -------------------

        /// <summary>
        /// Called to pause all operations performed by the snap-in.
        /// </summary>

        void PauseOperate();

        //  --------------------
        //  ResumeOperate method
        //  --------------------

        /// <summary>
        /// Called to resume all operations performed by the snap-in.
        /// </summary>

        void ResumeOperate();
    }
}

// eof "IServiceSnapIn"
