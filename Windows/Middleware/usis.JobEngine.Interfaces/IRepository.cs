//
//  @(#) IRepository.cs
//
//  Project:    usis Job Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System.ServiceModel;

namespace usis.JobEngine
{
    //  ---------------------
    //  IRepository interface
    //  ---------------------

    [ServiceContract]
    public interface IRepository
    {
        //  ---------------
        //  StartJob method
        //  ---------------

        [OperationContract]
        void StartJob(string key);
    }
}

// eof "IRepository.cs"
