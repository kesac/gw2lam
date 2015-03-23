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
    class Program
    {
        static void Main(string[] args)
        {

            Setup();

            MapManager maps = new MapManager();
            maps.InitializeLocalCache();

            Player playerData = Gw2PositionReaderApi.GetPlayerDataInstance();

            MusicManager musicManager = new MusicManager(MusicMode.Offline);
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

                        string mapName = maps.GetName(mapID);

                        if (mapName != null)
                        {
                            music.Playlist = musicManager.GetTracks(maps.GetName(mapID));

                            if (music.Playlist.Count > 0)
                            {
                                music.PlayRandomTrack();
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

                    Thread.Sleep(200);
                }

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            /**/
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

        private static void UpdateConsole(MusicPlayer music, Player playerData, string mapName)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("Guild Wars 2 Location-Aware Music");
            Console.WriteLine("=================================");

            double x = Math.Floor(playerData.AvatarPosition.X);
            double y = Math.Floor(playerData.AvatarPosition.Y);
            double z = Math.Floor(playerData.AvatarPosition.Z);
            
            Console.WriteLine("  Player: " + playerData.CharacterName);
            Console.WriteLine("Location: " + mapName + " (" + x + ", " + y + ", " + z + ")");
            Console.WriteLine("    Tick: " + playerData.Tick);
            Console.WriteLine();

            string[] tokens = music.TargetAudioFile.Split('\\');
            Console.WriteLine("   Track: " + tokens[tokens.Length-1]);
            Console.WriteLine("Duration: " + music.CurrentPosition + " / " + music.CurrentLength);
            Console.WriteLine("  Volume: " + Math.Floor(music.Volume * 100) + "%");
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

    }

}