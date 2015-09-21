using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Turtlesort.Glam.Core
{
    public class Settings
    {
        public Dictionary<string, object> data { get; private set; }

        public Settings()
        {
            this.data = new Dictionary<string, object>();
        }

        public void SaveToFile(string filename){
            string rawData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filename, rawData);
        }

        public void LoadFromFile(string filename)
        {
            using (StreamReader streamReader = new StreamReader(filename, Encoding.UTF8))
            {
                string rawData = streamReader.ReadToEnd();
                this.data = JsonConvert.DeserializeObject<Dictionary<string, object>>(rawData);
            }
        }

    }
}
