//
//  @(#) Engine.cs
//
//  Project:    usis Workflow Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016-2018 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using usis.Framework.Data.Entity;

namespace usis.Workflow.Engine
{
    //  ------------
    //  Engine class
    //  ------------

    internal sealed class Engine : DBContextModel<DBContext>
    {
        #region NewContext method

        //  -----------------
        //  NewContext method
        //  -----------------

        protected override DBContext NewContext()
        {
            if (DataSource?.ConnectionString == null) throw new EngineException(Strings.NoDataSourceConfigured);
            return new DBContext(DataSource?.ConnectionString);
        }

        #endregion NewContext method

        #region process definitions

        //  ------------------------------
        //  CreateProcessDefinition method
        //  ------------------------------

        internal ProcessDefinitionData CreateProcessDefinition(string name)
        {
            return UsingContext((db) =>
            {
                var p = new ProcessDefinition() { Id = Guid.NewGuid(), Name = name };
                db.ProcessDefinitions.Add(p);
                db.SaveChanges();
                return CreateProcessDefinitionData(db.ProcessDefinitions.Find(p.Id));
            });
        }

        //  ------------------------------
        //  QueryProcessDefinitions method
        //  ------------------------------

        internal IEnumerable<ProcessDefinitionData> QueryProcessDefinitions()
        {
            using (var db = NewContext())
            {
                var query = from pd in db.ProcessDefinitions
                            where pd.Deleted == 0
                            select pd;
                foreach (var pd in query) yield return CreateProcessDefinitionData(pd);
            }
        }

        //  ------------------------------
        //  DeleteProcessDefinition method
        //  ------------------------------

        internal void DeleteProcessDefinition(ProcessDefinitionId id)
        {
            Change((db) =>
            {
                var pd = db.ProcessDefinitions.Find(id.Value);
                if (pd != null) pd.Deleted++;
            });
        }

        #endregion process definitions

        #region private methods

        //  -------------
        //  Change method
        //  -------------

        private void Change(Action<DBContext> action)
        {
            UsingContext((db) =>
            {
                action.Invoke(db);
                db.SaveChanges();
            });
        }

        //  ----------------------------------
        //  CreateProcessDefinitionData method
        //  ----------------------------------

        private static ProcessDefinitionData CreateProcessDefinitionData(ProcessDefinition entity)
        {
            if (entity == null) return null;
            return new ProcessDefinitionData()
            {
                Id = new ProcessDefinitionId(entity.Id),
                Name = entity.Name,
                State = entity.State,
                Description = entity.Description,
                Created = entity.Created,
                Changed = entity.Changed
            };
        }

        #endregion private methods
    }

    #region EngineException class

    //  ---------------------
    //  EngineException class
    //  ---------------------

    [Serializable]
    public class EngineException : Exception
    {
        public EngineException() { }

        public EngineException(string message) : base(message) { }

        public EngineException(string message, Exception inner) : base(message, inner) { }

        protected EngineException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    #endregion EngineException class
}

// eof "Engine.cs"
