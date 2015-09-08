using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Turtlesort.Glam.Core
{
    // Represents either a music track on the local disk (ie. audio file) or
    // on the web
    public class MusicTrack
    {

        public uint MapId { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        public string Id
        {
            get
            {
                return this.Path.Split('=')[1];
            }
        }

        public MusicTrack(uint mapId, string path, string title)
        {
            this.MapId = mapId;
            this.Path = path;
            this.Title = title;
        }

    }
}
