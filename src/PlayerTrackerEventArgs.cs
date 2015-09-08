using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gw2lam
{
    public class PlayerTrackerEventArgs : EventArgs
    {
        public uint MapID { get; set; }
        public uint Tick { get; set; }

        public PlayerTrackerEventArgs(uint mapID, uint tick)
        {
            this.MapID = mapID;
            this.Tick = tick;
        }
    }

}
