//
//  @(#) Attribute.cs
//
//  Project:    usis Workflow Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016-2018 usis GmbH. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace usis.Workflow
{
    //  ---------------
    //  Attribute class
    //  ---------------

    /// <summary>
    /// Represents an attribute of a workflow entity.
    /// </summary>

    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    [DataContract]
    public sealed class Attribute { }
}

// eof "Attribute.cs"
