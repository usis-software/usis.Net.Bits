using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace usis.Server.CsvRouter
{
    internal class Channel : IDisposable
    {
        private FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

        public Channel(string path, string filter, bool recursive)
        {
            try
            {
                fileSystemWatcher.Path = path;
                fileSystemWatcher.Filter = filter;
                fileSystemWatcher.IncludeSubdirectories = recursive;
            }
            catch (ArgumentException exception)
            {
                Trace.TraceError(exception.Message);
            }
            fileSystemWatcher.Created += FileSystemWatcher_Created;
        }

        public void Dispose()
        {
            fileSystemWatcher.Dispose();
        }

        public bool StartWatching()
        {
            try
            {
                fileSystemWatcher.EnableRaisingEvents = true;
                return true;
            }
            catch (ArgumentException exception)
            {
                Trace.TraceError(exception.Message);
                return false;
            }
        }

        public void StopWatching()
        {
            fileSystemWatcher.EnableRaisingEvents = false;
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                Trace.TraceInformation("File '{0}' created.", e.Name);

                var tempFile = Path.GetTempFileName();
                File.Move(e.FullPath, tempFile);
            }
        }

        internal void ProcessFiles()
        {
            foreach (var file in FileSystem.WalkFiles(
                fileSystemWatcher.Path,
                fileSystemWatcher.Filter,
                fileSystemWatcher.IncludeSubdirectories))
            {
                Console.WriteLine(file);
            }
        }
    }

    internal static class FileSystem
    {
        internal static IEnumerable<string> WalkFiles(string path, string filter, bool recursive)
        {
            IEnumerable<string> enumerator = null;
            try
            {
                enumerator = Directory.EnumerateFiles(path, filter);
            }
            catch (UnauthorizedAccessException)
            {
                Debug.Print("*** Access denied: {0}", path);
                yield break;
            }
            foreach (var file in enumerator)
            {
                yield return file;
            }
            if (recursive)
            {
                foreach (var directory in Directory.EnumerateDirectories(path))
                {
                    foreach (var file in WalkFiles(directory, filter, true))
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}
