//
//  @(#) CommandLine.cs
//
//  Project:    usis.Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;

namespace usis.Platform
{
    #region CommandLine class

    //  -----------------
    //  CommandLine class
    //  -----------------

    /// <summary>
    /// Initializes and provides access to a collection of
    /// <see cref="CommandLineArgument"/> objects to handle the command
    /// line arguments of an application.
    /// </summary>
    /// <remarks>
    /// The static property <see cref="Arguments"/> can be used
    /// without any previous call to the <see cref="Initialize()"/>
    /// method (see sample code below).
    /// </remarks>
    /// <example>
    /// The following lines of code shows how to iterate thru all command
    /// line arguments passed to the current process:
    /// <code>
    /// public class CommandLineTest
    /// {
    ///     [STAThread]
    ///     static void Main(string[] args)
    ///     {
    ///         foreach (CommandLineArgument arg in CommandLine.Arguments)
    ///         {
    ///             Console.WriteLine(string.Format("Name = \"{0}\"", arg.Name));
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>

    public static class CommandLine
    {
        #region constants

        internal const string switchCharacters = "/-";
        internal const string assignCharacters = "=";

        #endregion

        #region private fields

        private static CommandLineArgumentCollection arguments;

        #endregion

        #region public methods

        //  -----------------
        //  Initialize method
        //  -----------------

        /// <overloads>
        /// Initializes the collections of command line arguments that is
        /// returned by the <see cref="Arguments"/> property.
        /// </overloads>
        /// <summary>
        /// Initializes the collections of command line arguments with
        /// the command line of the current process.
        /// </summary>

        public static void Initialize()
        {
            arguments = new CommandLineArgumentCollection(Environment.GetCommandLineArgs(), 1);
        }

        /// <summary>
        /// Initializes the collections of command line arguments from
        /// an array of strings.
        /// </summary>
        /// <param name="args">
        /// An array that contains the command line argument strings.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>args</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static void Initialize(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            arguments = new CommandLineArgumentCollection(args, 0);
        }

        #endregion public methods

        #region public properties

        //  ------------------
        //  Arguments property
        //  ------------------

        /// <summary>
        /// Returns a collection of <see cref="CommandLineArgument"/> objects
        /// representing the command line arguments passed to the process.
        /// </summary>
        /// <value>
        /// A <see cref="CommandLineArgumentCollection"/> that contains the
        /// command line arguments passed to the process.
        /// </value>
        /// <remarks>
        /// The collection is automatically initialized when this property is
        /// first accessed without a previous call to <see cref="Initialize()"/>.
        /// </remarks>

        public static CommandLineArgumentCollection Arguments
        {
            get
            {
                // default initialization
                if (arguments == null)
                {
                    Initialize();
                }
                return arguments;
            }
        }

        #endregion public properties
    }

    #endregion CommandLine class

    #region CommandLineArgument class

    //  -------------------------
    //  CommandLineArgument class
    //  -------------------------

    /// <summary>
    /// Represents a command line argument.
    /// </summary>

    public class CommandLineArgument
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal CommandLineArgument(string text)
        {
            Debug.Assert(text != null && text.Length != 0);

            Text = text;

            // test for switch characters
            IsSwitched = Text.IndexOfAny(CommandLine.switchCharacters.ToCharArray()) == 0;

            // extract name and value
            string[] s = Text.Remove(0, IsSwitched ? 1 : 0).Split(CommandLine.assignCharacters.ToCharArray(), 2);
            Debug.Assert(s.Length == 1 || s.Length == 2);

            if (s.Length == 1)
            {
                if (IsSwitched)
                {
                    Name = s[0];
                    Value = string.Empty;
                }
                else
                {
                    Name = string.Empty;
                    Value = s[0];
                }
            }
            else
            {
                Name = s[0];
                Value = s[1];
            }
        }

        #endregion construction

        #region public properties

        //  -------------
        //  Text property
        //  -------------

        /// <summary>
        /// Gets the complete text of a command line argument.
        /// </summary>

        public string Text
        {
            get;
            private set;
        }

        //  -------------------
        //  IsSwitched property
        //  -------------------

        /// <summary>
        /// Gets a value that indicates whether the command line argument is
        /// preceeded by a switch character
        /// ('<b>/</b>' or '<b>-</b>').
        /// </summary>

        public bool IsSwitched
        {
            get;
            private set;
        }

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Get the name of the command line argument.
        /// </summary>

        public string Name
        {
            get;
            private set;
        }

        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets the value of the command line argument.
        /// </summary>

        public string Value
        {
            get;
            private set;
        }

        #endregion public properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "CommandLineArgument: Name=\"{0}\", Value=\"{1}\", IsSwitched=\"{2}\"",
                Name, Value, IsSwitched);
        }

        #endregion overrides
    }

    #endregion CommandLineArgument class

    #region CommandLineArgumentCollection class

    //  -----------------------------------
    //  CommandLineArgumentCollection class
    //  -----------------------------------

    /// <summary>
    /// Represents a collection of <see cref="CommandLineArgument"/> objects.
    /// </summary>
    /// <remarks>
    /// The handling of command line argument names is case-insensitive.
    /// </remarks>

    public class CommandLineArgumentCollection : ICollection, ICollection<CommandLineArgument>
    {
        #region private fields

        private List<CommandLineArgument> arguments;
        private readonly HybridDictionary dictionary = new HybridDictionary(true);

        #endregion

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Creates a CommandLineArgumentCollection
        /// from the application's command line.
        /// </summary>

        public CommandLineArgumentCollection() : this(Environment.GetCommandLineArgs(), 1) { }

        /// <summary>
        /// Creates a CommandLineArgumentCollection from an array of arguments.
        /// </summary>
        /// <param name="args">
        /// An array of command line argument strings.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>args</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public CommandLineArgumentCollection(string[] args) : this(args, 0) { }

        internal CommandLineArgumentCollection(string[] args, int startIndex)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            Parse(args, startIndex);
        }

        #endregion construction

        #region public methods

        //  ---------------
        //  Contains method
        //  ---------------

        /// <summary>
        /// Checks whether a command line argument with the specified name
        /// exists in the collection.
        /// </summary>
        /// <param name="name">
        /// The name of the command line argument to look for.
        /// </param>
        /// <returns>
        /// <b>true</b> if the specified command line argument has been found,
        /// otherwise <b>false</b>.
        /// </returns>

        public bool Contains(string name)
        {
            return dictionary.Contains(name);
        }

        //  -----------------
        //  IsSwitched method
        //  -----------------

        /// <summary>
        /// Checks whether the specified command line argument
        /// is preceeded by a switch character ('<b>/</b>' or '<b>-</b>').
        /// </summary>
        /// <param name="name">
        /// The name of the command line argument.
        /// </param>
        /// <returns>
        /// <b>true</b> if the specified command line argument
        /// is preceeded by a switch charcater, otherwise <b>false</b>.
        /// </returns>

        public bool IsSwitched(string name)
        {
            CommandLineArgument arg = this[name];
            if (arg == null) return false;

            return arg.IsSwitched;
        }

        //  ---------------------------
        //  FindSwitchedArgument method
        //  ---------------------------

        /// <summary>
        /// Returns the command line argument with the specified name if it is
        /// preceeded by a switch character ('<b>/</b>' or '<b>-</b>').
        /// </summary>
        /// <param name="name">
        /// The name of the command line argument.
        /// </param>
        /// <returns>
        /// The command line argument with the specified name if it is switched.
        /// </returns>

        public CommandLineArgument FindSwitchedArgument(string name)
        {
            CommandLineArgument argument = this[name];
            if (argument != null && argument.IsSwitched) return argument;
            else return null;
        }

        //  -----------------------
        //  GetArgumentValue method
        //  -----------------------

        /// <summary>
        /// Gets the value of the command line argument with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the command line argument.
        /// </param>
        /// <param name="switched">
        /// A value that determines whether the command line argument must be
        /// preceeded by a switch character ('<b>/</b>' or '<b>-</b>').
        /// </param>
        /// <param name="defaultValue">
        /// A default value to be returned, when the no command line argument
        /// with the specified name exists or when the command line argument
        /// is not switched.
        /// </param>
        /// <returns>
        /// The value of the command line argument or the default value.
        /// </returns>

        public string GetArgumentValue(string name, bool switched, string defaultValue)
        {
            CommandLineArgument argument = this[name];
            if (argument != null)
            {
                if (argument.IsSwitched == switched)
                {
                    return argument.Value;
                }
            }
            return defaultValue;
        }

        #endregion public methods

        #region private methods

        //  ------------
        //  Parse method
        //  ------------

        private void Parse(string[] args, int startIndex)
        {
            arguments = new List<CommandLineArgument>(args.Length - startIndex);
            for (int i = startIndex; i < args.Length; i++)
            {
                if (args[i].Length == 0) continue;

                CommandLineArgument argument = new CommandLineArgument(args[i]);
                if (argument.Name.Length != 0)
                {
                    dictionary[argument.Name] = argument;
                }
                arguments.Add(argument);
            }
        }

        #endregion private methods

        #region ICollection members

        //  -----------------------
        //  IsSynchronized property
        //  -----------------------

        /// <summary>
        /// Gets a value indicating whether access to the
        /// <see cref="CommandLineArgumentCollection"/> is synchronized
        /// (thread-safe).
        /// </summary>

        public bool IsSynchronized
        {
            get
            {
                return ((ICollection)arguments).IsSynchronized;
            }
        }

        //  --------------
        //  Count property
        //  --------------

        /// <summary>
        /// Gets the number of command line arguments contained in the
        /// <see cref="CommandLineArgumentCollection"/>.
        /// </summary>

        public int Count
        {
            get
            {
                return arguments.Count;
            }
        }

        //  -------------------------
        //  ICollection.CopyTo method
        //  -------------------------

        /// <summary>
        /// Copies the elements of the <see cref="CommandLineArgumentCollection"/>
        /// to an <see cref="Array"/>,
        /// starting at a particular <b>Array</b> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the
        /// elements copied from <see cref="CommandLineArgumentCollection"/>.
        /// The <b>Array</b> must have zero-based indexing.
        /// </param>
        /// <param name="index">
        /// The zero-based index in <i>array</i> at which copying begins.
        /// </param>

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)arguments).CopyTo(array, index);
        }

        //  -------------
        //  CopyTo method
        //  -------------

        /// <summary>
        /// Copies the elements of the <see cref="CommandLineArgumentCollection"/>
        /// to an on <see cref="CommandLineArgument"/> array,
        /// starting at a particular array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="CommandLineArgument"/> array that
        /// is the destination of the elements copied from
        /// <see cref="CommandLineArgumentCollection"/>.
        /// The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <i>array</i> at which copying begins.
        /// </param>

        public void CopyTo(CommandLineArgument[] array, int arrayIndex)
        {
            arguments.CopyTo(array, arrayIndex);
        }

        //  -----------------
        //  SyncRoot property                                              |
        //  -----------------

        /// <summary>
        /// gets an object that can be used to synchronize access to the
        /// <see cref="CommandLineArgumentCollection"/>.
        /// </summary>

        public object SyncRoot
        {
            get
            {
                return ((ICollection)arguments).SyncRoot;
            }
        }

        #endregion ICollection members

        #region IEnumerable members

        //  --------------------
        //  GetEnumerator method
        //  --------------------

        /// <summary>
        /// Returns an enumerator that can iterate through the
        /// <see cref="CommandLineArgumentCollection"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> that can be used to iterate through
        /// the collection.
        /// </returns>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return arguments.GetEnumerator();
        }

        #endregion IEnumerable members

        #region indexers

        //  --------
        //  indexers
        //  --------

        /// <summary>
        /// Gets or sets the value associated with the specified key.<br/>
        /// In C#, this property is the indexer for the
        /// <see cref="CommandLineArgumentCollection"/> class.
        /// </summary>

        public CommandLineArgument this[int index]
        {
            get { return arguments[index]; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.<br/>
        /// In C#, this property is the indexer for the
        /// <see cref="CommandLineArgumentCollection"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <i>name</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public CommandLineArgument this[string name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return dictionary[name] as CommandLineArgument;
            }
        }

        #endregion indexers

        #region ICollection<CommandLineArgument> members

        //  ----------
        //  Add method
        //  ----------

        /// <summary>
        /// Adds an item to the <b>CommandLineArgumentCollection</b>.
        /// </summary>
        /// <param name="item">
        /// The <see cref="CommandLineArgument"/> item to add.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>item</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public void Add(CommandLineArgument item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            dictionary.Add(item.Name, item);
        }

        //  ------------
        //  Clear method
        //  ------------

        /// <summary>
        /// Removes all items from the <b>CommandLineArgumentCollection</b>.
        /// </summary>

        public void Clear()
        {
            dictionary.Clear();
        }

        //  ---------------
        //  Contains method
        //  ---------------

        /// <summary>
        /// Determines whether the <b>CommandLineArgumentCollection</b>
        /// contains a specific item.
        /// </summary>
        /// <param name="item">
        /// The <see cref="CommandLineArgument"/> to locate in the collection.
        /// </param>
        /// <returns>
        /// <b>true</b> if <i>item</i> is found in the collection;
        /// otherwise, <b>false</b>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <i>item</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public bool Contains(CommandLineArgument item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return dictionary.Contains(item.Name);
        }

        //  -------------------
        //  IsReadOnly property
        //  -------------------

        /// <summary>
        /// Gets a value indicating whether the
        /// <b>CommandLineArgumentCollection</b> is read-only.
        /// </summary>

        public bool IsReadOnly
        {
            get { return dictionary.IsReadOnly; }
        }

        //  -------------
        //  Remove method
        //  -------------

        /// <summary>
        /// Removes the first occurrence of a specific item from the
        /// <b>CommandLineArgumentCollection</b>.
        /// </summary>
        /// <param name="item">
        /// The <see cref="CommandLineArgument"/> to remove from the collection.
        /// </param>
        /// <returns>
        /// <b>true</b> if item was successfully removed from the collection;
        /// otherwise, <b>false</b>.
        /// This method also returns <b>false</b> if <i>item</i> is not found
        /// in the original collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <i>item</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public bool Remove(CommandLineArgument item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Contains(item))
            {
                dictionary.Remove(item.Name);
                return true;
            }
            else return false;
        }

        #endregion ICollection<CommandLineArgument> members

        #region IEnumerable<CommandLineArgument> members

        //  ----------------------------------------------
        //  IEnumerable<CommandLineArgument>.GetEnumerator
        //  ----------------------------------------------

        /// <summary>
        /// Returns an enumerator that can iterate through the
        /// <see cref="CommandLineArgumentCollection"/>.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>

        public IEnumerator<CommandLineArgument> GetEnumerator()
        {
            return arguments.GetEnumerator() as IEnumerator<CommandLineArgument>;
        }

        #endregion IEnumerable<CommandLineArgument> members
    }

    #endregion CommandLineArgumentCollection class
}

// eof "CommandLine.cs"
