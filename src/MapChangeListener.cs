using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GwApiNET.Gw2PositionReader;
using GwApiNET.Logging;

namespace Turtlesort.Glam.Core
{
    /*
     * Uses GwApiNET's position reader to detect when the player enters or exits a map
     */
    public class MapChangeListener
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

        public event MapChangeEventHandler OnUpdateStop;
        public event MapChangeEventHandler OnUpdateStart;
        public event MapChangeEventHandler OnMapChange;

        public MapChangeListener()
        {
            this.isRunning = false;
            this.PlayerData = Gw2PositionReaderApi.GetPlayerDataInstance();
        }


        public void Start()
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
                this.currentThread = new Thread(new ThreadStart(Run));
                this.currentThread.Start();
            }
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
                            MapChangeEventArgs e = new MapChangeEventArgs(mapID, tick);
                            this.OnUpdateStop(this, e);
                        }
                    }
                }

                if (mapID != PlayerData.MapId)
                {
                    mapID = this.PlayerData.MapId;
                    if (this.OnMapChange != null)
                    {
                        MapChangeEventArgs e = new MapChangeEventArgs(mapID, tick);
                        this.OnMapChange(this, e);
                    }
                }

                if(!incomingUpdates)
                {
                    if (tick != PlayerData.Tick)
                    {
                        incomingUpdates = true;
                        if (this.OnUpdateStart != null) 
                        { 
                            MapChangeEventArgs e = new MapChangeEventArgs(mapID, tick);
                            this.OnUpdateStart(this, e);
                        }
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

        /*
         * GwApiNET outputs log files that can't seem to be turned off.
         * They can be manually deleted by calling this method.
         */
        public void CleanUpLogFiles()
        {
            // Clean-up log files
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (string f in files)
            {
                string[] tokens = f.Split('\\');
                string filename = tokens[tokens.Length - 1];
                System.Diagnostics.Debug.WriteLine(filename);
                if (filename.StartsWith("GwApiNETLog") && filename.EndsWith(".txt"))
                {
                    File.Delete(filename);
                }
            }
        }
        
    }
}
