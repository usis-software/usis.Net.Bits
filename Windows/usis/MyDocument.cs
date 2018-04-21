using usis.Windows.Framework;

namespace usis.Windows
{
    class MyDocument : DocumentBase
    {
        public MyDocument(string fileName)
        {
            this.Title = fileName;
        }
    }
}
