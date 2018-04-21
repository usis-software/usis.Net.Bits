using System;
using System.Collections.Generic;
using System.Windows;

namespace usis.Windows.Framework
{
	//	-------------------------
	//	DocumentApplication class
	//	-------------------------

	public class DocumentApplication : Application
	{
		private List<IDocument> documents = new List<IDocument>();

		public event EventHandler<DocumentEventArgs> DocumentOpened;
		public event EventHandler<DocumentEventArgs> DocumentClosed;

		public void OpenDocument(IDocument document)
		{
			this.documents.Add(document);
			if (this.DocumentOpened != null) this.DocumentOpened(this, new DocumentEventArgs(document));
		}

		public void CloseDocument(IDocument document)
		{
			if (document == null) throw new ArgumentNullException("document");
			this.documents.Remove(document);
			if (this.DocumentClosed != null) this.DocumentClosed(this, new DocumentEventArgs(document));
		}

	} // DocumentApplication class

}
