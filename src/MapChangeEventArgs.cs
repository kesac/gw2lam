using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turtlesort.Glam.Core
{
    public class MapChangeEventArgs : EventArgs
    {
        public uint MapID { get; set; }
        public uint Tick { get; set; }

        public MapChangeEventArgs(uint mapID, uint tick)
        {
            this.MapID = mapID;
            this.Tick = tick;
        }
    }

}
