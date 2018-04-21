//
//  @(#) ICommand.cs
//
//  Project:    usis Mobile App Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

namespace usis.Mobile
{
    //  ------------------
    //  ICommand interface
    //  ------------------

    public interface ICommand
    {
        //  -------------------
        //  Controller property
        //  -------------------

        object Controller { get; set; }

        //  --------------
        //  Execute method
        //  --------------

        void Execute(params object[] parameters);
    }
}

// eof "ICommand.cs"
