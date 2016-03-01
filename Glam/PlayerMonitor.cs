﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glam;

namespace Glam
{
    public delegate void MapChangeEventHandler(object sender, MapChangeEventArgs e);

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

    public class PlayerMonitor
    {
        public bool IsRunning { get; private set; }
        public MumbleLink MumbleLink { get; set; }
        
        /* OnUpdateStart is not a reliable way to detect when the player enters a new map as
         * updates from GW2 can start without the map ID changing on map changes. Instead use
         * OnMapChange.
         */
        public event MapChangeEventHandler OnUpdateStart;
        public event MapChangeEventHandler OnUpdateStop;
        public event MapChangeEventHandler OnMapChange;

        public int CheckFrequency { get; set; }

        private Thread CurrentThread;

        public PlayerMonitor()
        {
            this.IsRunning = false;
            this.MumbleLink = new MumbleLink();

            this.CheckFrequency = 1000; //ms
        }

        public uint GetCurrentMap()
        {
            return this.MumbleLink.MapId;
        }

        public void Start()
        {
            if (!this.IsRunning)
            {
                this.IsRunning = true;
                this.CurrentThread = new Thread(new ThreadStart(Run));
                this.CurrentThread.Start();
            }
        }

        private void Run()
        {
            bool incomingUpdates = false;
            DateTime timeSinceLastTick = DateTime.Now;

            uint tick = 0;
            uint mapID = 0;

            while (this.IsRunning)
            {

                if (tick == MumbleLink.Tick && (DateTime.Now - timeSinceLastTick).Milliseconds >= this.CheckFrequency)
                {
                    if (incomingUpdates)
                    {
                        incomingUpdates = false;
                        if (this.OnUpdateStop != null)
                        {
                            MapChangeEventArgs e = new MapChangeEventArgs(mapID, tick);
                            this.OnUpdateStop(this, e);
                        }
                    }
                }

                if (mapID != MumbleLink.MapId)
                {
                    mapID = this.MumbleLink.MapId;
                    if (this.OnMapChange != null)
                    {
                        MapChangeEventArgs e = new MapChangeEventArgs(mapID, tick);
                        this.OnMapChange(this, e);
                    }
                }

                if(!incomingUpdates)
                {
                    if (tick != MumbleLink.Tick)
                    {
                        incomingUpdates = true;
                        if (this.OnUpdateStart != null) 
                        { 
                            MapChangeEventArgs e = new MapChangeEventArgs(mapID, tick);
                            this.OnUpdateStart(this, e);
                        }
                    }
                }

                if (tick != MumbleLink.Tick)
                {
                    tick = this.MumbleLink.Tick;
                    timeSinceLastTick = DateTime.Now;
                }
                
            }
        }


        public void Stop()
        {
            this.IsRunning = false;
        }

    }
}
