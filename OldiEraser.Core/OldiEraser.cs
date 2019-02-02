using Newtonsoft.Json;
using OldiEraser.Common;
using OldiEraser.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core
{
    public class OldiEraser : Disposable 
    {
        private readonly IOldieConfiguration configuration;
        private AppData AppData { get; set;}

        public OldiEraser(IOldieConfiguration configuration)
        {
            this.configuration = configuration;
            AppData = LoadAppData();
        }


        private AppData LoadAppData()
        {
            string dataFile = configuration.DataFilePath;
            if (File.Exists(dataFile))
                return AppData.Default;
            try
            {
                using (var reader = new StreamReader(dataFile))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<AppData>(json);
                }
            }
            catch(IOException)
            {
                // if something does not load right then file is broken. 
                // We will use default file in that situation
                return AppData.Default;
            }
        }

        protected override void FreeManagedObjects()
        {
            SaveAppData();
        }

        private void SaveAppData()
        {
            try
            {
                string dataFile = configuration.DataFilePath;
                using (var writer = new StreamWriter(dataFile))
                {
                    var json = JsonConvert.SerializeObject(AppData, Formatting.Indented);
                    writer.Write(json);
                }
            }
            catch (IOException e)
            {
                throw new Exception("Could not save data!", e);
            }
        }
    }
}
