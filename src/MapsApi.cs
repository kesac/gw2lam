using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace gw2lam
{
    // Requests maps data from the Guild Wars 2 official API (V2).
    // Call Load() to get a dictionary of Map data (map ids are the keys, and
    // Map classes are the values).
    //
    // By default, this class will store data retrieved from the API in a local cache so
    // that the number of API calls is reduced. To disable caching, set the EnableLocalCache
    // to false before calling Load().
    public class MapsApi
    {
        private const string RequestMapIds =  "https://api.guildwars2.com/v2/maps";
        private const string RequestMapData = "https://api.guildwars2.com/v2/maps?ids=";
        private const string LocalCacheFileName = "maps.cache.json";

        public bool EnableLocalCache { get; set; }

        public MapsApi()
        {
            this.EnableLocalCache = true;
        }

        /*
         * Makes requests to the Maps API and obtains data for all existing GW2 maps
         */
        public Dictionary<uint, Map> Load()
        {

            Dictionary<uint, Map> maps = new Dictionary<uint, Map>();

            if (this.EnableLocalCache && File.Exists(LocalCacheFileName))
            {
                using (StreamReader streamReader = new StreamReader(LocalCacheFileName, Encoding.UTF8))
                {
                    string rawData = streamReader.ReadToEnd();
                    List<Map> mapsData = JsonConvert.DeserializeObject<List<Map>>(rawData);
                    foreach (Map m in mapsData)
                    {
                        maps[m.Id] = m;
                    }
                }
                System.Console.WriteLine("Local Cache Loaded! (" + maps.Count + " elements)");
            }
            else
            {
                // First we need to know which maps exist...
                WebClient client = new WebClient();
                string mapIdsResponse = client.DownloadString(RequestMapIds);
                List<string> mapIds = JsonConvert.DeserializeObject<List<string>>(mapIdsResponse);

                // Then we need to query the API for each map's ID. Luckily we can request more than
                // one map at a time, but unfortunately, we cannot request all at once. (Returns 404
                // if the number of map ID arguments provided through GET is too large).
                int groupSize = 25;
                for (int i = 0; i < mapIds.Count; i += groupSize)
                {
                    StringBuilder idsQuery = new StringBuilder();
                    for (int j = i; j < i + groupSize && j < mapIds.Count; j++)
                    {
                        idsQuery.Append(",");
                        idsQuery.Append(mapIds[j]);
                    }

                    string mapDataResponse = client.DownloadString(RequestMapData + idsQuery);
                    List<Map> mapsData = JsonConvert.DeserializeObject<List<Map>>(mapDataResponse);

                    foreach (Map m in mapsData)
                    {
                        maps[m.Id] = m;
                    }

                }

                if (this.EnableLocalCache)
                {
                    string rawData = JsonConvert.SerializeObject(maps.Values);
                    File.WriteAllText(LocalCacheFileName, rawData);
                }
                System.Console.WriteLine("Obtained over web! (" + maps.Count + " elements)");
            }

            return maps;
        }

    }
}
