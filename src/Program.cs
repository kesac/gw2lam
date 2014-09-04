using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using GwApiNET.Gw2PositionReader;
using GwApiNET.ResponseObjects;
using GwApiNET.Logging;

using NAudio;
using NAudio.Wave;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace gw2lam
{

    public class MapData
    {
        public string map_name { get; set; }
    }

    public class APIResponse
    {
        public Dictionary<int, MapData> maps { get; set; }
    }

    class Program
    {

        private const string MAP_DATA_FILE = "maps.json";
        private const string MAP_DATA_API = "http://api.guildwars2.com/v1/maps.json";


        static void Main(string[] args)
        {
            //OldProgram.Start();

            
            // Create our look-up table to convert map IDs to names
            string rawData;
            if (!File.Exists(MAP_DATA_FILE)) // Download maps.json if it doesn't exist
            {
                WebClient client = new WebClient();
                rawData = client.DownloadString(MAP_DATA_API);
                File.WriteAllText(MAP_DATA_FILE, rawData);
            }
            else
            {
                using (StreamReader streamReader = new StreamReader(MAP_DATA_FILE, Encoding.UTF8))
                {
                    rawData = streamReader.ReadToEnd();
                }
            }

            APIResponse response = JsonConvert.DeserializeObject<APIResponse>(rawData);
            Dictionary<int, MapData> maps = response.maps;

            // Create a music folder if it doesn't exist
            if (!Directory.Exists("music"))
            {
                Directory.CreateDirectory("music");
            }

            // MainMenu is a special folder. gw2lam cannot detect if a player is at the main 
            // login screen through the mumble API and so must rely on GW2's native
            // custom music feature that is handled via playlist files.
            // The only problem is now we have to silence all other music
            /*
            if (Directory.Exists("music\\Login Screen"))
            {
                List<string> mp3s = GetMP3s("music\\Login Screen");

                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Guild Wars 2\\Music\\"))
                {
                    using (StreamWriter file = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Guild Wars 2\\Music\\MainMenu.m3u"))
                    {
                        file.WriteLine("#EXTM3U");
                        foreach (string line in mp3s)
                        {
                            file.WriteLine("#EXTINF:-1, " + line);
                            file.WriteLine(line);
                        }
                    }

                    using (StreamWriter file = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Guild Wars 2\\Music\\Ambient.m3u"))
                    {
                        file.WriteLine("#EXTM3U");
                    }
                }

            }
            //*/

            // Turn off all logging of the GwApiNET library
            foreach (string loggerName in GwApiNET.Constants.LoggerNames)
            {
                try
                {
                    foreach (GwLogManager.LogLevel logLevel in Enum.GetValues(typeof(GwLogManager.LogLevel)))
                    {
                        GwLogManager.SetLogLevel(loggerName, false, logLevel);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                
            }

            // Begin the main application loop
            MusicPlayer music = new MusicPlayer();
            Player playerData = Gw2PositionReaderApi.GetPlayerDataInstance();
            uint tick = 0;
            uint mapID = 0;

            do
            {
                while (!Console.KeyAvailable){
                    
                    UpdateConsole(music, playerData);

                    if (mapID != playerData.MapId)
                    {
                        // The map has changed, abruptly stop the music and start a new track
                        music.StopAudio();
                        mapID = playerData.MapId;

                        // If this map's name is not in our lookup table, we use the convention mapXXX
                        // where XXX is the ID
                        string path = "music\\map" + mapID;
                        if (maps.ContainsKey((int)mapID))
                        {
                            path = "music\\" + maps[(int)mapID].map_name;
                        }

                        if (Directory.Exists(path))
                        {
                            List<string> mp3s = GetMP3s(path);

                            if (mp3s.Count > 0)
                            {
                                Random random = new Random();
                                string bgm = mp3s[random.Next(mp3s.Count)];
                                music.PlayAudio(bgm);
                            }
                        }

                        tick--; // This is to prevent a fadeout during the next iteration because the tick hasn't change yet even though the map just has

                    }
                    else if (tick == playerData.Tick && music.IsPlaying)
                    {
                        // The ticks have stopped updating. We are likely in a map transition.
                        // Fade out instead of abruptly stopping the music.
                        music.FadeStop();
                    }
                    else if (tick != playerData.Tick && mapID == playerData.MapId && !music.IsPlaying) // The music has finished and a new track must be selected.
                    {
                        // This will force the next iteration of this loop to select a new track.
                        // If there is only one track associated with the map, this will effectively
                        // loop the track.
                        mapID = 0;
                    }


                    music.Update();
                    tick = playerData.Tick;

                    Thread.Sleep(100);
                }

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        private static void UpdateConsole(MusicPlayer music, Player playerData)
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("Guild Wars 2 Custom Music Player");
            Console.WriteLine("================================");
            
            Console.WriteLine("Player: " + playerData.CharacterName);
            Console.WriteLine("Version: " + playerData.Version);
            Console.WriteLine("Y: " + playerData.AvatarPosition.Y);
            Console.WriteLine("X: " + playerData.AvatarPosition.X);
            Console.WriteLine("Z: " + playerData.AvatarPosition.Z);
            Console.WriteLine("");
            Console.WriteLine("Tick: " + playerData.Tick);
            Console.WriteLine("Currently Playing: " + music.TargetAudioFile);
            Console.WriteLine("Volume: " + (music.Volume * 100));
        }


        private static List<string> GetMP3s(string directory)
        {
            string[] files = Directory.GetFiles(Path.GetFullPath(directory));
            

            List<string> mp3s = new List<string>();

            foreach (string f in files)
            {
                if (f.EndsWith(".mp3"))
                {
                    mp3s.Add(f);
                }
            }

            return mp3s;
        }

    }

}