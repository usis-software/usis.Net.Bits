//
//	@(#) SnapInHost.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace usis.Framework
{
	#region SnapInHost class

	//	----------------
	//	SnapInHost class
	//	----------------

	internal class SnapInHost
	{
		#region fields

		private Collection<SnapInCollectionItem> registeredSnapIns = new Collection<SnapInCollectionItem>();

		#endregion fields

		#region methods

		#region RegisterSnapIn method

		//	---------------------
		//	RegisterSnapIn method
		//	---------------------
		/// <summary>
		/// Registers the specified type as a snap-in.  
		/// </summary>
		/// <param name="type">
		/// The type of the snap-in.
		/// </param>

		internal void RegisterSnapIn(Type type)
		{
			this.registeredSnapIns.Add(new SnapInCollectionItem(type));
		
		} // RegisterSnapIn method

		#endregion RegisterSnapIn method

		#region LoadSnapIns method

		//	------------------
		//	LoadSnapIns method
		//	------------------

		internal void LoadSnapIns(ISolution solution)
		{
			foreach (SnapInCollectionItem item in this.registeredSnapIns)
			{
				item.Load(solution);
			}
		
		} // LoadSnapIns method

		#endregion LoadSnapIns method

		#region ConnectSnapIns method

		//	---------------------
		//	ConnectSnapIns method
		//	---------------------

		internal void ConnectSnapIns(ISolution solution)
		{
			var items = new List<SnapInCollectionItem>(this.registeredSnapIns);
			foreach (SnapInCollectionItem item in items)
			{
				item.Connect(solution);
			}
		
		} // ConnectSnapIns method

		#endregion ConnectSnapIns method

		#region DisconnectSnapIns method

		//	------------------------
		//	DisconnectSnapIns method
		//	------------------------

		internal void DisconnectSnapIns(ISolution solution)
		{
			foreach (SnapInCollectionItem item in this.registeredSnapIns)
			{
				item.Disconnect(solution);
			}
		}

		#endregion DisconnectSnapIns method

		#region UnloadSnapIns method

		//	--------------------
		//	UnloadSnapIns method
		//	--------------------

		internal void UnloadSnapIns(ISolution solution)
		{
			foreach (SnapInCollectionItem item in this.registeredSnapIns)
			{
				item.Unload(solution);
			}

		} // UnloadSnapIns method
	
		#endregion UnloadSnapIns method

		#region ConnectRequiredSnapIn method

		//	----------------------------
		//	ConnectRequiredSnapIn method
		//	----------------------------

		internal bool ConnectRequiredSnapIn(ISolution solution, ISnapIn instance, Type type)
		{
			var newItem = new SnapInCollectionItem(type);
			int i = -1;
			foreach (var item in this.registeredSnapIns)
			{
				i++; 
				if (item.IsEqualInstance(instance)) break;
			}
			if (i >= 0) this.registeredSnapIns.Insert(i, newItem);
			else this.registeredSnapIns.Add(newItem);

			return newItem.Connect(solution);

		} // ConnectRequiredSnapIn method

		#endregion ConnectRequiredSnapIn method

		#endregion methods

	} // SnapInHost class

	#endregion SnapInHost class

	#region SnapInCollectionItem class

	//	--------------------------
	//	SnapInCollectionItem class
	//	--------------------------

	internal class SnapInCollectionItem
	{
		#region fields

		private Type type;
		private ISnapIn instance;
		private bool connected;

		#endregion fields

		#region properties

		//	-----------------
		//	IsLoaded property
		//	-----------------
		/// <summary>
		/// Gets a value that indicates whether the snap-in is loaded.
		/// </summary>

		internal bool IsLoaded
		{
			get
			{
				return this.instance != null;
			}
		}

		#endregion properties

		#region construction

		//	--------------------------------
		//	SnapInCollectionItem constructor
		//	--------------------------------

		internal SnapInCollectionItem(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			this.type = type;

		} // SnapInCollectionItem constructor

		#endregion construction

		#region Load method

		//	-----------
		//	Load method
		//	-----------

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		internal bool Load(ISolution solution)
		{
			if (!this.IsLoaded)
			{
				try
				{
					// ToDo: Verify that type derives from ISnapIn
					this.instance = Activator.CreateInstance(this.type) as ISnapIn;
					
					Debug.Print("Snap-in loaded: '{0}'", this.instance.ToString());
				}
				catch (Exception exception)
				{
					this.ReportException(solution, exception, Strings.FailedToLoadSnapIn);
				}
			}
			return this.IsLoaded;

		} // Load method

		#endregion Load method

		#region Connect method

		//	--------------
		//	Connect method
		//	--------------

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		internal bool Connect(ISolution solution)
		{
			if (this.Load(solution))
			{
				if (!this.connected)
				{
					try
					{
						this.connected = this.instance.OnConnection(solution);
						
						Debug.Print("Snap-in connected: '{0}'", this.instance.ToString());
					}
					catch (Exception exception)
					{
						this.ReportException(solution, exception, Strings.FailedToConnectSnapIn);
					}
				}
			}
			return this.connected;
		
		} // Connect method

		#endregion Connect method

		#region Disconnect method

		//	-----------------
		//	Disconnect method
		//	-----------------

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		internal bool Disconnect(ISolution solution)
		{
			if (this.connected)
			{
				try
				{
					this.instance.OnDisconnection();
					this.connected = false;

					Debug.Print("Snap-in disconnected: '{0}'", this.instance.ToString());
				}
				catch (Exception exception)
				{
					this.ReportException(solution, exception, Strings.FailedToDisconnectSnapIn);
				}
			}
			return !this.connected;

		} // Disconnect method

		#endregion Disconnect method

		#region Unload method

		//	-------------
		//	Unload method
		//	-------------

		internal bool Unload(ISolution solution)
		{
			if (this.IsLoaded)
			{
				if (this.Disconnect(solution))
				{
					string name = this.instance.ToString();

					// dispose snap-in instance
					IDisposable disposable = this.instance as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}

					// release instance
					this.instance = null;

					Debug.Print("Snap-in unloaded: '{0}'", name);
				}
			}
			return !this.IsLoaded;

		} // Unload method

		#endregion Unload method

		#region IsEqualInstance method

		//	----------------------
		//	IsEqualInstance method
		//	----------------------

		internal bool IsEqualInstance(ISnapIn snapIn)
		{
			return this.instance == snapIn;

		} // IsEqualInstance method

		#endregion IsEqualInstance method

		private void ReportException(ISolution solution, Exception exception, string message)
		{
			string format = string.Format(
				CultureInfo.CurrentCulture, message,
				this.instance.ToString(), "{0}");
			solution.UseExtension<EventLogExtension>().Write(
				exception, CultureInfo.CurrentCulture, format);

			Debug.Print(format, exception.Message);
		}

	} // SnapInCollectionItem class

	#endregion SnapInCollectionItem class

} // usis.Framework namespace

// eof "SnapInHost.cs"
