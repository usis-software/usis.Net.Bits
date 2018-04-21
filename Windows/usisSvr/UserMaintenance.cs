using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using usis.Framework;

namespace usis.Server.Services
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	internal class UserMaintenance : IUserMaintenance
    {
        void IUserMaintenance.RegisterByEmail()
        {
            Debug.Print("IUserRegistration.RegisterUser()");

			using (var db = new usis.DbContext())
			{
				//var person = new usis.Solution.Person()
				//{
				//	FirstName = "Udo",
				//	LastName = "Schäfer"
				//};
				//db.Persons.Add(person);
				//db.SaveChanges();

				foreach (var contact in db.Contacts)
				{
					Debug.Print(contact.DisplayName);
				}
			}
        }
    }
}
