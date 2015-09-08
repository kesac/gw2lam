using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gw2lam
{
    // Represents either a music track on the local disk (ie. audio file) or
    // on the web
    public class MusicTrack
    {
        public uint MapId { get; set; }
        public string Path { get; set; }
    }
}
