using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.Text;
using GwApiNET.Gw2PositionReader;

namespace gw2lam.offline
{
    class OfflineStart
    {
        static void Main(string[] args)
        {
            PlayerTracker tracker = new PlayerTracker();
            MapManagerOld mapManager = new MapManagerOld();
            mapManager.InitializeLocalCache();

            MusicManager musicManager = new MusicManager(MusicMode.Offline);
            MusicPlayer musicPlayer = new MusicPlayer();

            string currentMap = "";
            uint tick = 0;
            uint mapID = 0;

            do
            {
                while (!Console.KeyAvailable){

                    UpdateConsole(musicPlayer, tracker.PlayerData, currentMap);

                    if (mapID != tracker.PlayerData.MapId)
                    {
                        // The map has changed, abruptly stop the music and start a new track
                        musicPlayer.StopAudio();
                        mapID = tracker.PlayerData.MapId;

                        string mapName = mapManager.GetName(mapID);

                        if (mapName != null)
                        {
                            musicPlayer.Playlist = musicManager.GetTracks(mapManager.GetName(mapID));

                            if (musicPlayer.Playlist.Count > 0)
                            {
                                musicPlayer.PlayRandomTrack();
                            }
                        }
                        
                        tick--; // This is to prevent a fadeout during the next iteration because the tick hasn't change yet even though the map just has

                    }
                    else if (tick == tracker.PlayerData.Tick && musicPlayer.IsPlaying)
                    {
                        // The ticks have stopped updating. We are likely in a map transition.
                        // Fade out instead of abruptly stopping the music.
                        musicPlayer.FadeStop();
                    }
                    else if (tick != tracker.PlayerData.Tick && mapID == tracker.PlayerData.MapId && !musicPlayer.IsPlaying) // The music has finished and a new track must be selected.
                    {
                        // This will force the next iteration of this loop to select a new track.
                        // If there is only one track associated with the map, this will effectively
                        // loop the track.
                        mapID = 0;
                    }

                    musicPlayer.Update();
                    tick = tracker.PlayerData.Tick;

                    Thread.Sleep(200);
                }

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            tracker.CleanUpLogFiles();
            /**/
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