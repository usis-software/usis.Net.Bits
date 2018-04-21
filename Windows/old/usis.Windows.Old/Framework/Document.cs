using System;

namespace usis.Windows.Framework
{
	//	-------------------
	//	IDocument interface
	//	-------------------

	public interface IDocument
	{
		string Title { get; }
	
	} // IDocument interface

	//	-----------------------
	//	DocumentEventArgs class
	//	-----------------------

	public class DocumentEventArgs : EventArgs
	{
		public IDocument Document
		{
			get;
			private set;
		}

		public DocumentEventArgs(IDocument document)
		{
			if (document == null) throw new ArgumentNullException("document");
			this.Document = document;
		}

	} // DocumentEventArgs class
}
