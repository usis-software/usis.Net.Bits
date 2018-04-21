//
//  @(#) SnapInActivator.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;

namespace usis.Framework
{
    //  ---------------------
    //  SnapInActivator class
    //  ---------------------

    /// <summary>
    /// Provides a class to create a snap-in instance.
    /// </summary>

    public class SnapInActivator
    {
        //  ---------------------
        //  CreateInstance method
        //  ---------------------

        /// <summary>
        /// Creates an instance of a snap-in.
        /// </summary>
        /// <param name="application">The application in which the snap-in instance is created.</param>
        /// <param name="context">The context to create the snap-in instance.</param>
        /// <returns>The created snap-in instance.</returns>
        /// <exception cref="ArgumentNullException"><i>context</i> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="TypeLoadException"></exception>

        public virtual object CreateInstance(IApplication application, SnapInActivatorContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Type snapInType = null;
            object instance = null;

            if (!string.IsNullOrWhiteSpace(context.TypeName))
            {
                snapInType = Type.GetType(context.TypeName, true);
            }
            else if (context.SnapInType != null)
            {
                snapInType = context.SnapInType;
            }
            else throw new InvalidOperationException();

            // create instance from type
            if (snapInType != null) instance = Activator.CreateInstance(snapInType);

            //  failed to create instance
            if (instance == null) throw new TypeLoadException();

            return instance;
        }
    }

    //  ----------------------------
    //  SnapInActivatorContext class
    //  ----------------------------

    /// <summary>
    /// Provides information about the snap-in instance to create.
    /// </summary>

    public class SnapInActivatorContext
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapInActivatorContext"/> class.
        /// </summary>
        /// <param name="snapInType">The type of the instance to create.</param>
        /// <param name="typeName">The full type name of the instance to created.</param>
        /// <param name="assemblyFile">The assembly file that contains the type of the instance to create.</param>

        public SnapInActivatorContext(Type snapInType, string typeName, string assemblyFile)
        {
            SnapInType = snapInType;
            TypeName = typeName;
            AssemblyFile = assemblyFile;
        }

        #endregion construction

        #region properties

        //  -------------------
        //  SnapInType property
        //  -------------------

        /// <summary>
        /// Gets the type of the instance to create.
        /// </summary>
        /// <value>
        /// The type of the instance to create.
        /// </value>

        public Type SnapInType { get; }

        //  -----------------
        //  TypeName property
        //  -----------------

        /// <summary>
        /// Gets the full type name of the instance to created.
        /// </summary>
        /// <value>
        /// The full type name of the instance to created.
        /// </value>

        public string TypeName { get; }

        //  ---------------------
        //  AssemblyFile property
        //  ---------------------

        /// <summary>
        /// Gets the assembly file that contains the type of the instance to create.
        /// </summary>
        /// <value>
        /// The assembly file that contains the type of the instance to create.
        /// </value>

        public string AssemblyFile { get; }

        #endregion properties
    }
}

// eof "SnapInActivator.cs"
