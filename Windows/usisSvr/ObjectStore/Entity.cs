using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using usis.Solution;

namespace usis.ObjectStore
{
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	internal class Entity : BaseEntity
	{
		[Key]
		public Guid EntityId
		{
			get; set;
		}
	}
}

namespace usis.Solution
{
	partial class DbContext
	{
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public DbSet<usis.ObjectStore.Entity> Entities
		{
			get; set;
		}
	}
}
