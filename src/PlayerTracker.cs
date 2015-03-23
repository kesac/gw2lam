using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GwApiNET.Gw2PositionReader;

namespace gw2lam
{

    public delegate void PlayerTrackerEventHandler(object sender, PlayerTrackerEventArgs e);

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

    public class PlayerTracker
    {

        private Thread currentThread;

        public bool isRunning { get; private set; }
        public Player PlayerData {get; set;}
        public string PlayerName
        {
            get
            {
                return this.PlayerData.CharacterName;
            }
        }

        public event PlayerTrackerEventHandler OnUpdateStop;
        public event PlayerTrackerEventHandler OnUpdateStart;
        public event PlayerTrackerEventHandler OnMapChange;

        public PlayerTracker()
        {
            this.isRunning = false;
            this.PlayerData = Gw2PositionReaderApi.GetPlayerDataInstance();
        }

        public void Start()
        {
            this.isRunning = true;
            this.currentThread = new Thread(new ThreadStart(Run));
            this.currentThread.Start();
        }

        private void Run()
        {
            bool incomingUpdates = false;
            DateTime timeSinceLastTick = DateTime.Now;

            uint tick = 0;
            uint mapID = 0;

            while (this.isRunning)
            {

                if (tick == PlayerData.Tick && (DateTime.Now - timeSinceLastTick).Seconds > 1)
                {
                    if (incomingUpdates)
                    {
                        incomingUpdates = false;
                        if (this.OnUpdateStop != null)
                        {
                            PlayerTrackerEventArgs e = new PlayerTrackerEventArgs(mapID, tick);
                            this.OnUpdateStop(this, e);
                        }
                    }
                }

                if(!incomingUpdates)
                {
                    if (tick != PlayerData.Tick)
                    {
                        incomingUpdates = true;
                        if (this.OnUpdateStart != null) 
                        { 
                            PlayerTrackerEventArgs e = new PlayerTrackerEventArgs(mapID, tick);
                            this.OnUpdateStart(this, e);
                        }
                    }
                }

                if (mapID != PlayerData.MapId)
                {
                    mapID = this.PlayerData.MapId;
                    if (this.OnMapChange != null)
                    {
                        PlayerTrackerEventArgs e = new PlayerTrackerEventArgs(mapID, tick);
                        this.OnMapChange(this, e);
                    }
                }

                if (tick != PlayerData.Tick)
                {
                    tick = this.PlayerData.Tick;
                    timeSinceLastTick = DateTime.Now;
                }
                
            }
        }


        public void Stop()
        {
            this.isRunning = false;
        }
        
    }
}
