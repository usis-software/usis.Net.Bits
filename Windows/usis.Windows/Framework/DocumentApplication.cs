using System;
using System.Collections.Generic;
using System.Windows;

namespace usis.Windows.Framework
{
	//	-------------------------
	//	DocumentApplication class
	//	-------------------------

	[Obsolete]
	public class DocumentApplication : Application
	{
		#region fields

		private List<IDocument> documents = new List<IDocument>();

		#endregion fields

		#region events

		public event EventHandler<DocumentEventArgs> DocumentOpened;
		public event EventHandler<DocumentEventArgs> DocumentClosed;

		#endregion events

		#region public methods

		//	-------------------
		//	OpenDocument method
		//	-------------------

		public void OpenDocument(IDocument document)
		{
			if (document == null) throw new ArgumentNullException("document");

			this.documents.Add(document);
			if (this.DocumentOpened != null) this.DocumentOpened(this, new DocumentEventArgs(document));
		
		} // OpenDocument method

		//	--------------------
		//	CloseDocument method
		//	--------------------

		public void CloseDocument(IDocument document)
		{
			if (document == null) throw new ArgumentNullException("document");

			this.documents.Remove(document);
			if (this.DocumentClosed != null) this.DocumentClosed(this, new DocumentEventArgs(document));
		
		} // CloseDocument method

		#endregion public methods

		#region public properties

		public static new DocumentApplication Current
		{
			get
			{
				return Application.Current as DocumentApplication;
			}
		}

		#endregion public properties

	} // DocumentApplication class

} // usis.Windows.Framework namespace
