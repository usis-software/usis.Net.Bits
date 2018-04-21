using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using usis.Framework;

namespace usis.Solution
{
	public class DbContextSnapIn : usis.Framework.SnapIn
	{
		public static void RegisterDbContextInterface(ISolution solution, Type interfaceType)
		{
			var extension = solution.Extensions.Find<DbContextSolutionExtension>();
			if (extension == null)
			{
				extension = new DbContextSolutionExtension();
				solution.Extensions.Add(extension);
			}
			extension.RegisterInterface(interfaceType);
		}

		public static void RegisterDbContext<TContext>(ISolution solution)
		{
			DbContextSnapIn.RegisterDbContextInterface(solution, typeof(TContext));
		}

		//protected override void OnConnecting(CancelEventArgs e)
		//{
		//	using (var db = new DbContext())
		//	{
		//		//foreach (var user in db.Users)
		//		//{
		//		//	Debug.Print(user.LoginName);
		//		//}
		//		//Debug.Print("Database '{0}' connected.", db.Database.Connection.Database);
		//	}
		//}
	}

	public class DbContextSolutionExtension : IExtension<ISolution>
	{
		private List<PropertyInfo> propertyList = new List<PropertyInfo>();

		internal void RegisterInterface(Type interfaceType)
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
						this.propertyList.Add(propertyMember);
						Debug.Print("- entity type: {0}", propertyMember.PropertyType.GenericTypeArguments[0].FullName);
					}
				}
			}
		}

		void IExtension<ISolution>.Attach(ISolution owner)
		{
		}

		void IExtension<ISolution>.Detach(ISolution owner)
		{
		}
	}
}
