using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using usis.Framework.Entity;
using usis.Platform.Data;

namespace usis.Finance
{
    //  ---------------
    //  DBContext class
    //  ---------------

    public sealed class DBContext : DBContextBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DBContext(DataSource dataSource) : base(dataSource) { }

        #endregion construction

        #region properties

        public DbSet<FinancialAccount> Accounts { get; set; }

        public DbSet<FinancialTransaction> Transactions { get; set; }

        public DbSet<FinancialSecurity> Securities { get; set; }

        public DbSet<FinancialSecurityTransaction> SecurityTransactions { get; set; }

        #endregion properties
    }

    #region FinancialAccount class

    //  ----------------------
    //  FinancialAccount class
    //  ----------------------

    public sealed class FinancialAccount : EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Currency Currency { get; set; }
    }

    #endregion FinancialAccount class

    #region FinancialTransaction class

    //  --------------------------
    //  FinancialTransaction class
    //  --------------------------

    public class FinancialTransaction : EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public virtual FinancialAccount Account { get; set; }

        public TransactionType Kind { get; set; }

        public DateTime Date { get; set; }

        public string Text { get; set; }

        public decimal Amount { get; set; }

        public int Currency { get; set; }
    }

    #endregion FinancialTransaction class

    #region FinancialSecurity class

    //  -----------------------
    //  FinancialSecurity class
    //  -----------------------

    public sealed class FinancialSecurity : EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Column("ISIN")]
        public string Isin { get; set; }
    }

    #endregion FinancialSecurity class

    #region FinancialSecurityTransaction class

    //  ----------------------------------
    //  FinancialSecurityTransaction class
    //  ----------------------------------

    public class FinancialSecurityTransaction
    {
        [Key]
        [Column(Order = 0)]
        public Guid TransactionId { get; set; }

        public virtual FinancialTransaction Transaction { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid SecurityId { get; set; }

        public virtual FinancialSecurity Security { get; set; }
    }

    #endregion FinancialSecurityTransaction class
}
