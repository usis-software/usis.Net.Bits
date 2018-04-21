using System;
using System.Data.Entity;
using usis.Framework.Entity;

namespace usis.Finance
{
    //  -----------
    //  Model class
    //  -----------

    internal sealed class Model : DBContextModel<DBContext>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public Model()
        {
            Database.SetInitializer(new DatabaseInitializer());
            IsLoggingEnabled = true;
        }

        #endregion construction

        #region overrides

        //  -----------------
        //  NewContext method
        //  -----------------

        protected override DBContext NewContext()
        {
            if (DataSource == null) throw new InvalidOperationException();
            return new DBContext(DataSource);
        }

        #endregion overrides
    }

    internal static class DBContextExtensions
    {
        public static FinancialAccount CreateAccount(this DBContext context, string name, Currency currency)
        {
            var account = new FinancialAccount() { Id = Guid.NewGuid(), Name = name, Currency = currency };
            context.Accounts.Add(account);
            return account;
        }

        //public static FinSecurity CreateSecurity(this DBContext context, string isin, string name)
        //{
        //    throw new NotImplementedException();
        //}
    }

    internal class DatabaseInitializer : DropCreateDatabaseAlways<DBContext> // DropCreateDatabaseIfModelChanges<DBContext>
    {
        protected override void Seed(DBContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            /*var account1 = */context.CreateAccount("ebase", Currency.Eur);
            /*var account2 = */context.CreateAccount("Templeton", Currency.Usd);
            /*var account3 = */context.CreateAccount("Union", Currency.Eur);
            /*var account4 = */context.CreateAccount("VoBa private", Currency.Eur);
            /*var account5 = */context.CreateAccount("VoBa usis", Currency.Eur);

            //var security01 = new FinancialSecurity() { Id = Guid.NewGuid(), Isin = "FR0010135103", Name = "Carmignac Patrimoine" };
            //context.Securities.Add(security01);
            //var security02 = new FinancialSecurity() { Id = Guid.NewGuid(), Isin = "LU0114760746", Name = "Franklin Templeton - Growth (Euro) Fund-A-acc" };
            //context.Securities.Add(security02);

            //using (var dbTransaction = context.Database.BeginTransaction())
            //{
            //    var transaction = new FinancialTransaction() { Id = Guid.NewGuid(), AccountId = account3.Id, Kind = TransactionType.CashDeposit, Text = "Bareinzahlung", Amount = 1160 };
            //    context.Transactions.Add(transaction);
            //    //var securityTransaction = new SecurityTransaction() { TransactionId = transaction.Id, SecurityId = security.Id };
            //    //context.SecurityTransactions.Add(securityTransaction);
            //}
        }
    }
}
