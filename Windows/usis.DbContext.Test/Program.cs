using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace usis.Test
{
	public class Contact
	{
		public Guid ContactId
		{
			get;
			set;
		}
		public string DisplayName
		{
			get;
			set;
		}
	}

	public class Task
	{
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

	interface ITestDbContext : IDisposable
	{
		string Test
		{
			get;
			set;
		}
		DbSet<Contact> Contacts
		{
			get;
		}
		DbSet<Task> Tasks
		{
			get;
		}
	}

	public class TestDbContext : DbContext, ITestDbContext
	{
		public string Test
		{
			get; set;
		}

		public DbSet<Contact> Contacts
		{
			get;
			set;
		}


		public DbSet<Task> Tasks
		{
			get;
			set;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			if (modelBuilder == null) throw new ArgumentNullException("modelBuilder");

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			base.OnModelCreating(modelBuilder);
		}
	}

	public class DbContextSnapIn
	{
		public static void RegisterDbContextInterface(ISolution solution, Type interfaceType)
		{
			if (!interfaceType.IsInterface) throw new ArgumentException();

			foreach (var member in interfaceType.GetMembers())
			{
				if (member.MemberType == MemberTypes.Property)
				{
					Debug.Print("Property '{0}'", member.Name);

					var propertyMember = member as PropertyInfo;
					Debug.Print("- type: {0}", propertyMember.PropertyType.FullName);
					if (!propertyMember.PropertyType.IsGenericType) continue;
					Debug.Print("- base type: {0}", propertyMember.PropertyType.GetGenericTypeDefinition());

					if (propertyMember.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
					{
						Debug.Print("ok");
					}
				}
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<TestDbContext>());
			DbContextSnapIn.RegisterDbContextInterface(null, typeof(ITestDbContext));

			using (var db = new TestDbContext())
			{
				foreach (var contact in db.Contacts)
				{
					Console.WriteLine(string.Format("Contact {0}: {1}", contact.ContactId.ToString(), contact.DisplayName));
				}
			}

			Console.Write("Press any key to continue ... ");
			Console.ReadKey(true);
		}
	}

	public interface ISolution
	{
	}
}
