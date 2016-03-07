using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Glam
{
    public class Map
    {
        public uint Id { get; set; }
        public string Name { get; set; }
    }

    public class MapService
    {
        private const string RequestMapIds =  "https://api.guildwars2.com/v2/maps";
        private const string RequestMapData = "https://api.guildwars2.com/v2/maps?ids=";

        public string CacheFileName { get; set; }
        public bool CreateLocalCache { get; set; }
        public int ReadGroupSize { get; set; }
        public bool CacheExists { get { return File.Exists(CacheFileName); } }

        private Dictionary<uint, Map> Maps;

        public MapService()
        {
            this.Maps = new Dictionary<uint, Map>();

            // Default values
            this.CacheFileName = "map.cache.json";
            this.CreateLocalCache = true;
            this.ReadGroupSize = 25;
        }
        
        public void LoadFromCache()
        {
            if (this.CacheExists)
            {
                using (StreamReader streamReader = new StreamReader(CacheFileName, Encoding.UTF8))
                {
                    string rawData = streamReader.ReadToEnd();
                    List<Map> mapsData = JsonConvert.DeserializeObject<List<Map>>(rawData);
                    foreach (Map m in mapsData)
                    {
                        this.Maps[m.Id] = m;
                    }
                }
            }
        }

        public void LoadFromWebApi()
        {
            
            // First we need to know which maps exist...
            WebClient client = new WebClient();
            string mapIdsResponse = client.DownloadString(RequestMapIds);
            List<string> mapIds = JsonConvert.DeserializeObject<List<string>>(mapIdsResponse);

            // Then we need to query the API for each map's ID. Luckily we can request more than
            // one map at a time, but unfortunately, we cannot request all at once. (Returns 404
            // if the number of map ID arguments provided through GET is too large).

            for (int i = 0; i < mapIds.Count; i += this.ReadGroupSize)
            {
                StringBuilder idsQuery = new StringBuilder();
                for (int j = i; j < i + this.ReadGroupSize && j < mapIds.Count; j++)
                {
                    idsQuery.Append(",");
                    idsQuery.Append(mapIds[j]);
                }

                string mapDataResponse = client.DownloadString(RequestMapData + idsQuery);
                List<Map> mapsData = JsonConvert.DeserializeObject<List<Map>>(mapDataResponse);

                foreach (Map m in mapsData)
                {
                    this.Maps[m.Id] = m;
                }

            }

            if (this.CreateLocalCache)
            {
                string rawData = JsonConvert.SerializeObject(this.Maps.Values);
                File.WriteAllText(CacheFileName, rawData);
            }
            

        }

        public string ResolveName(uint id)
        {
            if (this.Maps.Keys.Contains(id))
            {
                return this.Maps[id].Name;
            }
            else
            {
                return "Map " + id.ToString();
            }
        }
    }
}
