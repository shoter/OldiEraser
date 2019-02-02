using System;
using System.Collections.Generic;
using System.Text;

namespace OldiEraser.Core
{
    public interface IOldFileEraser
    {
        /// <summary>
        /// Erases all old files in the directory according to settings file in that directory.
        /// If there is no setting file in the directory nothing should happen
        /// </summary>
        /// <param name="directoryPath">directory path where old file erasure will happen.</param>
        void EraseOldFiles(string directoryPath);
    }
}
