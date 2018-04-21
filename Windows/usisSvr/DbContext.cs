using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using usis.Framework;
using usis.Solution;

namespace usis
{
	internal class DbContextSnapIn : SnapIn
	{
		protected override void OnConnecting(CancelEventArgs e)
		{
			using (var db = new DbContext())
			{
				foreach (var user in db.Users)
				{
					Debug.Print(user.LoginName);
				}
				Debug.Print("Database '{0}' connected.", db.Database.Connection.Database);
			}
		}
	}
}
