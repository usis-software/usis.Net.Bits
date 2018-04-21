//
//  @(#) CommandLine.cs
//
//  Project:    usis.Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2018 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace usis.Platform
{
    //  -----------------
    //  CommandLine class
    //  -----------------

    /// <summary>
    /// Represents a set of arguments that where passed to an application.
    /// </summary>
    /// <example>
    /// The following lines of code show how to iterate through all command-line arguments passed to the current process:
    /// <code>
    /// using System;
    /// using usis.Platform;
    /// class CommandLineTest
    /// {
    /// static void Main(string[] args)
    /// {
    /// foreach (var arg in new CommandLine(args).Arguments)
    /// {
    /// Console.WriteLine(arg);
    /// }
    /// }
    /// }
    /// </code></example>

    public sealed class CommandLine
    {
        #region fields

        internal string optionCharacters;
        internal string assignCharacters;

        private List<CommandLineArgument> arguments;

        #endregion fields

        #region properties

        //  ------------------
        //  Arguments property
        //  ------------------

        /// <summary>
        /// Gets an enumeration to iterate through the command-line arguments.
        /// </summary>
        /// <value>
        /// The enumeration to iterate through the command-line arguments.
        /// </value>

        public IEnumerable<CommandLineArgument> Arguments => arguments;

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine" /> class
        /// with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>

        public CommandLine(params string[] arguments) : this("-/", "=", arguments) { }

        private CommandLine(string optionCharacters, string assignCharacters, params string[] arguments)
        {
            this.optionCharacters = optionCharacters;
            this.assignCharacters = assignCharacters;

            this.arguments = arguments.Select(a => new CommandLineArgument(this, a)).ToList();
        }

        #endregion construction

        #region methods

        //  ----------------
        //  HasOption method
        //  ----------------

        /// <summary>
        /// Determines whether the command line contains an option argument with the specified name.
        /// </summary>
        /// <param name="name">The name of the option argument.</param>
        /// <returns>
        ///   <c>true</c> if the command line contains an option argument with the specified name; otherwise, <c>false</c>.
        /// </returns>

        public bool HasOption(string name)
        {
            return arguments.Any(argument => argument.IsOption && argument.HasName(name));
        }

        //  ---------------
        //  GetValue method
        //  ---------------

        /// <summary>
        /// Gets the value of the command-line argument with the specified name.
        /// </summary>
        /// <param name="name">The name of the command-line argument.</param>
        /// <returns>
        /// The value of the command-line argument.
        /// </returns>

        public string GetValue(string name)
        {
            return arguments.FirstOrDefault(argument => argument.HasName(name))?.Value;
        }

        #endregion methods
    }

    #region CommandLineArgument class

    //  -------------------------
    //  CommandLineArgument class
    //  -------------------------

    /// <summary>
    /// Represents a command-line argument.
    /// </summary>

    public sealed class CommandLineArgument
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal CommandLineArgument(CommandLine commandLine, string text)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(text));

            Text = text;

            // test for option characters
            IsOption = Text.IndexOfAny(commandLine.optionCharacters.ToCharArray()) == 0;

            // extract name and value
            string[] s = Text.Remove(0, IsOption ? 1 : 0).Split(commandLine.assignCharacters.ToCharArray(), 2);
            Debug.Assert(s.Length == 1 || s.Length == 2);

            if (s.Length == 1)
            {
                if (IsOption)
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
        /// Gets the complete text of a command-line argument.
        /// </summary>
        /// <value>
        /// The text of the command-line argument.
        /// </value>

        public string Text { get; }

        //  -----------------
        //  IsOption property
        //  -----------------

        /// <summary>
        /// Gets a value that indicates whether the command-line argument is
        /// preceded by an option character
        /// ('<c>/</c>' or '<c>-</c>').
        /// </summary>
        /// <value>
        ///   <c>true</c> if this argument is preceded by an option character; otherwise, <c>false</c>.
        /// </value>

        public bool IsOption { get; }

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Get the name of the command-line argument.
        /// </summary>
        /// <value>
        /// The name of the command-line argument.
        /// </value>

        public string Name { get; }

        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets the value of the command-line argument.
        /// </summary>
        /// <value>
        /// The value of the command-line argument.
        /// </value>

        public string Value { get; }

        #endregion public properties

        #region internal methods

        //  --------------
        //  HasName method
        //  --------------

        internal bool HasName(string name)
        {
            return string.Equals(name, Name, StringComparison.OrdinalIgnoreCase);
        }

        #endregion internal methods

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
                "CommandLineArgument: Name=\"{0}\", Value=\"{1}\", IsOption=\"{2}\"",
                Name, Value, IsOption);
        }

        #endregion overrides
    }

    #endregion CommandLineArgument class
}

// eof "CommandLine.cs"
