using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Turtlesort.Glam.Core
{
    public class Map
    {
        public uint Id { get; set; }
        public string Name { get; set; }

        
        [JsonIgnore]
        public List<MusicTrack> Tracks {get; private set;}

        public Map()
        {
            this.Tracks = new List<MusicTrack>();
        }

    }
}
