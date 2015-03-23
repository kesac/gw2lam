using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace gw2lam
{

    public class MapData
    {
        public string map_name { get; set; }
    }

    public class APIResponse
    {
        public Dictionary<uint, MapData> maps { get; set; }
    }

    public class MapManager
    {

        private const string MAP_DATA_FILE = "maps.json";
        private const string MAP_DATA_API_V1 = "http://api.guildwars2.com/v1/maps.json";
        private const string MAP_DATA_API_V2 = "http://api.guildwars2.com/v2/maps/";

        private Dictionary<uint, MapData> mapData;

        public MapManager()
        {
            this.mapData = new Dictionary<uint, MapData>();
        }

        /// <summary>
        /// <para>
        /// Initializes MapManager's ID-to-name look-up table for maps from
        /// the 'maps.json' file.
        ///</para>
        ///<para>
        /// If the file does not already exist in the same folder as the
        /// executable, the file will be downloaded using GW2's V1 API. 
        ///</para>
        /// </summary>
        public void InitializeLocalCache()
        {
            
            string rawData;
            if (!File.Exists(MAP_DATA_FILE)) // Download maps.json if it doesn't exist
            {
                WebClient client = new WebClient();
                rawData = client.DownloadString(MAP_DATA_API_V1);
                File.WriteAllText(MAP_DATA_FILE, rawData);
            }
            else
            {
                using (StreamReader streamReader = new StreamReader(MAP_DATA_FILE, Encoding.UTF8))
                {
                    rawData = streamReader.ReadToEnd();
                }
            }

            APIResponse response = JsonConvert.DeserializeObject<APIResponse>(rawData);
            this.mapData = response.maps;
        }


        public string GetName(uint id)
        {
            if (this.mapData.ContainsKey(id))
            {
                return this.mapData[id].map_name;
            }
            else
            {
                // try to use the API
                return null;
            }
        }

        public bool IsValidID(uint id)
        {
            return this.mapData.ContainsKey(id) || false; // try to use api here
        }

    }
}
