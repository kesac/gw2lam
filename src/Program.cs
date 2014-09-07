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

        private static readonly string[] SUPPORTED_FORMATS = new string[]{ ".mp3", ".wav", ".ogg" };


        static void Main(string[] args)
        {
            Setup();

            Player playerData = Gw2PositionReaderApi.GetPlayerDataInstance();
            Dictionary<int, MapData> maps = GetMapsData();
            MusicPlayer music = new MusicPlayer();
            
            string currentMap = "";
            uint tick = 0;
            uint mapID = 0;

            do
            {
                while (!Console.KeyAvailable){

                    UpdateConsole(music, playerData, currentMap);

                    if (mapID != playerData.MapId)
                    {
                        // The map has changed, abruptly stop the music and start a new track
                        music.StopAudio();
                        mapID = playerData.MapId;

                        // If this map's name is not in our lookup table, we use the convention mapXXX
                        // where XXX is the ID
                        string path = "music\\map" + mapID;
                        currentMap = "map" + mapID;

                        if (maps.ContainsKey((int)mapID))
                        {
                            path = "music\\" + maps[(int)mapID].map_name;
                            currentMap = maps[(int)mapID].map_name;
                        }
                        

                        if (Directory.Exists(path))
                        {
                            music.Playlist = GetMusic(path);
                            music.PlayRandomTrack();
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

        private static void Setup()
        {
            // Create a music folder if it doesn't exist
            if (!Directory.Exists("music"))
            {
                Directory.CreateDirectory("music");
            }

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
        }

        private static Dictionary<int, MapData> GetMapsData()
        {
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
            return response.maps;
        }


        private static void UpdateConsole(MusicPlayer music, Player playerData, string mapName)
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("Guild Wars 2 Custom Music Player");
            Console.WriteLine("================================");
            Console.WriteLine("Player: " + playerData.CharacterName);
            
            double x = Math.Floor(playerData.AvatarPosition.X);
            double y = Math.Floor(playerData.AvatarPosition.Y);
            double z = Math.Floor(playerData.AvatarPosition.Z);

            Console.WriteLine("Current Location: " + mapName + " (" + x + ", " + y + ", " + z + ")");
            Console.WriteLine("Tick: " + playerData.Tick);
            Console.WriteLine();

            string[] tokens = music.TargetAudioFile.Split('\\');
            Console.WriteLine("Currently Playing: " + tokens[tokens.Length-1]);
            Console.WriteLine("Volume: " + Math.Floor(music.Volume * 100) + "%");
            Console.WriteLine("Duration: " + music.CurrentPosition + " / " + music.CurrentLength);
            Console.WriteLine();
            Console.WriteLine("Map Playlist:");

            if(music.Playlist != null && music.IsPlaying)
            {
                foreach (string item in music.Playlist)
                {
                    tokens = item.Split('\\');
                    Console.WriteLine("+ " + tokens[tokens.Length-1]);
                }
            }
        }


        private static List<string> GetMusic(string directory)
        {
            string[] files = Directory.GetFiles(Path.GetFullPath(directory));
            

            List<string> musicFiles = new List<string>();

            foreach (string f in files)
            {
                foreach (string format in SUPPORTED_FORMATS)
                {
                    if (f.EndsWith(format))
                    {
                        musicFiles.Add(f);
                        break;
                    }
                }
            }

            return musicFiles;
        }

    }

}