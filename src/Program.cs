using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using GwApiNET.Gw2PositionReader;
using GwApiNET.ResponseObjects;

using NAudio;
using NAudio.Wave;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace gw2lam
{

    public class Item
    {
        public string map_name { get; set; }
    }

    public class MapNamesResponse
    {
        public Dictionary<int, Item> maps { get; set; }
    }

    class Program
    {

        private static IWavePlayer waveOutDevice;
        private static AudioFileReader audioFileReader;
        private static string currentlyPlaying = "";

        static void Main(string[] args)
        {

            string rawMapData;
            using (StreamReader streamReader = new StreamReader("maps.json", Encoding.UTF8))
            {
                rawMapData = streamReader.ReadToEnd();
            }

            MapNamesResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<MapNamesResponse>(rawMapData);
            Dictionary<int, Item> maps = response.maps;

            // Setup
            if (!Directory.Exists("music"))
            {
                Directory.CreateDirectory("music");
            }

            uint currentMapID = 0;

            Player p = Gw2PositionReaderApi.GetPlayerDataInstance();
            do
            {
                while (!Console.KeyAvailable)
                {
                    Console.Clear();

                    Console.WriteLine("Guild Wars 2 Custom Music Player");
                    Console.WriteLine("================================");
                    Console.WriteLine("Player: " + p.CharacterName);

                    //if(maps.Keys.Contains((int)p.MapId))
                    if (maps.ContainsKey((int)p.MapId))
                    {
                        Console.WriteLine("Map: " + maps[(int)p.MapId].map_name);
                    }
                    else
                    {
                        Console.WriteLine("Map: " + p.MapId);
                    }

                    if (waveOutDevice != null && audioFileReader != null && audioFileReader.HasData(1))
                    {
                        Console.WriteLine("Currently Playing: " + currentlyPlaying);
                    }
                    else
                    {
                        Console.WriteLine("No custom music available for map");
                    }


                    if (currentMapID != p.MapId)
                    {
                        // stop audio
                        Program.StopAudio();

                        currentMapID = p.MapId;

                        string path = "music\\map" + currentMapID;

                        //if (maps.Keys.Contains((int)currentMapID))
                        if (maps.ContainsKey((int)currentMapID))
                        {
                            path = "music\\" + maps[(int)currentMapID].map_name;
                        }

                        if (Directory.Exists(path))
                        {
                            string[] files = Directory.GetFiles(path);

                            List<string> mp3s = new List<string>();

                            foreach (string f in files)
                            {
                                if (f.EndsWith(".mp3"))
                                {
                                    mp3s.Add(f);
                                }
                            }

                            if (mp3s.Count > 0)
                            {
                                Random random = new Random();
                                string bgm = mp3s[random.Next(mp3s.Count)];

                                PlayAudio(bgm);
                                Console.WriteLine(bgm);
                            }
                        }

                    }
                    // Loops the audio
                    else if (audioFileReader != null && !audioFileReader.HasData(1))
                    {
                        StopAudio();
                        currentMapID = 0;
                    }

                    Thread.Sleep(100);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        private static void PlayAudio(String path)
        {
            currentlyPlaying = path;
            Program.waveOutDevice = new WaveOut();
            Program.audioFileReader = new AudioFileReader(path);
            Program.waveOutDevice.Init(audioFileReader);
            Program.waveOutDevice.Play();
        }

        private static void StopAudio()
        {
            currentlyPlaying = "";

            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }
            if (waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
        }

    }

}