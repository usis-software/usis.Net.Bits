using System;

namespace usis.Data.Entity
{
	public interface IDbContext : IDisposable
	{
		int SaveChanges();
	}
}
