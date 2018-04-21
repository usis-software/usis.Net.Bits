using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace usis.Solution
{
	//public abstract class BaseEntity
	//{
	//	protected BaseEntity()
	//	{
	//		this.Created = DateTime.Now;
	//	}

	//	public DateTime Created
	//	{
	//		get;
	//		set;
	//	}

	//	public DateTime? Changed
	//	{
	//		get;
	//		set;
	//	}
	//}

	//public partial class DbContext : System.Data.Entity.DbContext
	//{
	//	protected override void OnModelCreating(DbModelBuilder modelBuilder)
	//	{
	//		if (modelBuilder == null) throw new ArgumentNullException("modelBuilder");

	//		modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
	//		base.OnModelCreating(modelBuilder);
	//	}

	//}
}