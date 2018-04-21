//
//  @(#) DBContext.cs
//
//  Project:    usis Workflow Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016-2018 usis GmbH. All rights reserved.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using usis.Framework.Data.Entity;

namespace usis.Workflow.Engine
{
    //  ---------------
    //  DBContext class
    //  ---------------

    internal class DBContext : DBContextBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DBContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            // to avoid code analysis warnings
            if (ProcessDefinitions == null) ProcessDefinitions = null;
        }

        #endregion construction

        #region properties

        //  ---------------------------
        //  ProcessDefinitions property
        //  ---------------------------

        public DbSet<ProcessDefinition> ProcessDefinitions { get; set; }

        #endregion properties
    }

    #region ProcessDefinition class

    //  -----------------------
    //  ProcessDefinition class
    //  -----------------------

    [Table(nameof(ProcessDefinition))]
    internal class ProcessDefinition : EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(63)]
        public string Name { get; set; }

        public ProcessDefinitionState State { get; set; }

        public string Description { get; set; }
    }

    #endregion ProcessDefinition class
}

// eof "DBContext.cs"
