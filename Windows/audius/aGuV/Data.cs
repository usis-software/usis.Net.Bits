//
//  @(#) Data.cs
//
//  Project:    audius GuV
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 audius GmbH. All rights reserved.

using System.Data.Linq.Mapping;
using System.Diagnostics.CodeAnalysis;

namespace audius.GuV.Data
{
    #region Position class

    //  --------------
    //  Position class
    //  --------------

    [Table(Name = "audiusGuVPositionen")]
    internal class Position
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Column(IsPrimaryKey = true)]
        internal int Id { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Column(CanBeNull = true)]
        internal string Name { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Column(Name ="Position",  CanBeNull = true)]
        internal int? Parent { get; set; }
    }

    #endregion Position class

    #region GeneralLedgerAccountRange class

    //  -------------------------------
    //  GeneralLedgerAccountRange class
    //  -------------------------------

    [Table(Name = "audiusGuVSachkonten")]
    internal class GeneralLedgerAccountRange
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Column(IsPrimaryKey = true)]
        internal int Id { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Column(Name = "Position")]
        internal int Position { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Column(Name = "SaKtoVon")]
        internal string From { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Column(Name = "SaKtoBis")]
        internal string To { get; set; }
    }

    #endregion GeneralLedgerAccountRange class
}

//  eof "Data.cs"
