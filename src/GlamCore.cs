using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Turtlesort.Glam.Core
{
    // Retrieves map data from the GW2 maps API
    // Retrieves track data locally
    // Provides a list of tracks provided a map id
    // Associates a new track to a map id
    public class GlamCore
    {

        private const string MusicTrackFileName = "music.tracks.json";

        private Dictionary<uint, Map> Maps;

        public GlamCore()
        {

        }

        public void Load()
        {
            if (this.Maps == null) { 
                MapsApi api = new MapsApi();
                api.EnableLocalCache = true;
                this.Maps = api.Read();

                if (File.Exists(MusicTrackFileName))
                {

                    using (StreamReader streamReader = new StreamReader(MusicTrackFileName, Encoding.UTF8))
                    {
                        string rawData = streamReader.ReadToEnd();
                        List<MusicTrack> tracks = JsonConvert.DeserializeObject<List<MusicTrack>>(rawData);

                        foreach (MusicTrack track in tracks)
                        {
                            if (this.Maps.ContainsKey(track.MapId))
                            {
                                this.Maps[track.MapId].Tracks.Add(track);
                            }
                        }
                    }

                }
            }
        }

        public string GetMapName(uint mapId)
        {
            return this.Maps[mapId].Name;
        }

        public List<MusicTrack> GetMapMusic(uint mapId)
        {
            return this.Maps[mapId].Tracks;
        }

        public void AddMapMusic(MusicTrack track)
        {
            this.Maps[track.MapId].Tracks.Add(track);
        }

        public void SaveTrackData()
        {
            List<MusicTrack> tracks = new List<MusicTrack>();

            foreach (Map map in this.Maps.Values)
            {
                tracks.AddRange(map.Tracks);
            }

            string rawData = JsonConvert.SerializeObject(tracks);
            File.WriteAllText(MusicTrackFileName, rawData);
        }

    }
}
