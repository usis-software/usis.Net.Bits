using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace usis.Solution
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	internal class User : BaseEntity
	{
		[Key]
		public Guid UserId
		{
			get;
			set;
		}

		public string LoginName
		{
			get;
			set;
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	internal class Task : BaseEntity
	{
		[Key]
		public Guid TaskId
		{
			get;
			set;
		}

		public string Subject
		{
			get;
			set;
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	internal class Contact : BaseEntity
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public Guid ContactId
		{
			get;
			set;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public string DisplayName
		{
			get;
			set;
		}
	}

	//internal class Person : Contact
	//{
	//	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	//	public string FirstName
	//	{
	//		get;
	//		set;
	//	}

	//	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	//	public string LastName
	//	{
	//		get;
	//		set;
	//	}
	//}

	internal abstract class BaseEntity
	{
		internal BaseEntity()
		{
			this.Created = DateTime.Now;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public DateTime Created
		{
			get;
			set;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public DateTime? Changed
		{
			get;
			set;
		}
	}

	internal partial class DbContext : System.Data.Entity.DbContext
	{
		public DbContext() : base("usis")
		{
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public DbSet<User> Users
		{
			get;
			set;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public DbSet<Task> Tasks
		{
			get;
			set;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public DbSet<Contact> Contacts
		{
			get;
			set;
		}

		//[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		//public DbSet<Person> Persons
		//{
		//	get;
		//	set;
		//}
	}
}
