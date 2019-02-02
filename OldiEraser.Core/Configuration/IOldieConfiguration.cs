using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Configuration
{
    public interface IOldieConfiguration
    {
       string DataFilePath { get; }
    }
}
