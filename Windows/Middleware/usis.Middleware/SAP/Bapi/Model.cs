#pragma warning disable 1591

using System.Collections.Generic;
using SAP.Middleware.Connector;
using System;

namespace usis.Middleware.SAP.Bapi
{
    public class BasisModel : Model, IUser
    {
        private const string BapiUserGetList = "BAPI_USER_GETLIST";

        #region construction

        [CLSCompliant(false)]
        public BasisModel(RfcDestination destination) : base(destination) { }

        #endregion construction

        #region IUser implementation

        public IEnumerable<User> GetList(int maxRows, bool withUserName)
        {
            var function = Destination.Repository.CreateFunction(BapiUserGetList);
            function.SetValue("MAX_ROWS", maxRows);
            if (withUserName) function.SetValue("WITH_USERNAME", "X");

            function.Invoke(Destination);

            foreach (var user in function.GetTable("USERLIST"))
            {
                yield return new User(user, withUserName);
            }
        }

        #endregion IUser implementation
    }

    #region Model class

    //  -----------
    //  Model class
    //  -----------

    public abstract class Model
    {
        #region properties

        //  --------------------
        //  Destination property
        //  --------------------

        [CLSCompliant(false)]
        protected RfcDestination Destination { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        [CLSCompliant(false)]
        protected Model(RfcDestination destination) { Destination = destination; }

        #endregion construction
    }

    #endregion Model class

    public interface IUser
    {
        IEnumerable<User> GetList(int maxRows, bool withUserName);
    }

    #region User class

    //  ----------
    //  User class
    //  ----------

    public class User
    {
        #region constants

        //  ---------------
        //  FieldName class
        //  ---------------

        private static class FieldName
        {
            internal const string USERNAME = nameof(USERNAME);
            internal const string FIRSTNAME = nameof(FIRSTNAME);
            internal const string LASTNAME = nameof(LASTNAME);
            internal const string FULLNAME = nameof(FULLNAME);
        }

        #endregion constants

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal User(IRfcStructure user, bool withUsername)
        {
            UserName = user.GetString(FieldName.USERNAME);
            if (withUsername)
            {
                FirstName = user.GetString(FieldName.FIRSTNAME);
                LastName = user.GetString(FieldName.LASTNAME);
                FullName = user.GetString(FieldName.FULLNAME);
            }
        }

        #endregion construction

        #region properties

        public string UserName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }

        #endregion properties
    }

    #endregion User class
}
