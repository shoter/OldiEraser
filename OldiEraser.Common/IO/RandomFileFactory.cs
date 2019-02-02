using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Common.IO
{
    public class RandomFileFactory
    {
        private readonly string directoryPath;

        public RandomFileFactory(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        /// <summary>
        /// Returns a file path to random file that was created by this factory
        /// </summary>
        public string CreateRandomFile()
        {
            Directory.CreateDirectory(directoryPath);
            while (true)
            {
                var filename = Path.GetRandomFileName();
                var filepath = Path.Combine(directoryPath, filename);
                if (File.Exists(filepath) == false)
                {
                    File.Create(filepath).Dispose();
                    return filepath;
                }
            }
        }
    }
}
