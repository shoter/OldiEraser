using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Settings
{
    public enum DirectoriesDeleteBehaviour
    {
        /// <summary>
        /// Directories will be not touched
        /// </summary>
        DoNothing,

        /// <summary>
        /// Old directories with whole content will be removed
        /// </summary>
        DeleteOldDirectories,

        /// <summary>
        /// All files inside directories (recursive) will be removed
        /// </summary>
        DeleteFilesInside
    }
}
