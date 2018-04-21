using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Web;
using usis.Framework;
using usis.Server.Services;

namespace usis.Framework
{
	//	----------------------------------------
	//	ISolutionAdministrationService interface
	//	----------------------------------------

	[ServiceContract]
	interface ISolutionAdministrationService
	{
		[OperationContract]
		[WebGet]
		int Test();
	
	} // ISolutionAdministrationService interface

	//	-----------------------------------
	//	SolutionAdministrationService class
	//	-----------------------------------

	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	internal class SolutionAdministrationService : SolutionObject, ISolutionAdministrationService
	{
		int ISolutionAdministrationService.Test()
		{
			Debug.WriteLine(this.Solution);
			
			var extension = this.Solution.Extensions.Find<SolutionSnapInHostExtension>();
			if (extension == null) return 0;

			return extension.SnapInHost.GetHashCode();
		}
	
	} // SolutionAdministrationService class

	#region SolutionSnapInHostExtension class

	//	---------------------------------
	//	SolutionSnapInHostExtension class
	//	---------------------------------

	internal class SolutionSnapInHostExtension : IExtension<ISolution>
	{
		//	-------------------
		//	SnapInHost property
		//	-------------------
		
		internal SnapInHost SnapInHost
		{
			get;
			private set;
		
		} // SnapInHost property

		//	-----------
		//	constructor
		//	-----------
		
		internal SolutionSnapInHostExtension(SnapInHost snapInHost)
		{
			if (snapInHost == null) throw new ArgumentNullException("snapInHost");
			this.SnapInHost = snapInHost;
		
		} // constructor

		#region IExtension<ISolution> methods

		void IExtension<ISolution>.Attach(ISolution owner)
		{
		}

		void IExtension<ISolution>.Detach(ISolution owner)
		{
		}

		#endregion IExtension<ISolution> methods
	
	} // SolutionSnapInHostExtension class

	#endregion SolutionSnapInHostExtension class

} // usis.Framework namespace
