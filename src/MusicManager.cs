using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gw2lam
{
    public enum MusicMode { Offline, Online };

    /// <summary>
    /// <para>
    /// In offline mode, this class will keep track of music files
    /// in the music subdirectory.
    /// </para>
    /// <para>
    /// In online mode, this class will read the online.ini within
    /// the music subdirectory.
    /// </para>
    /// </summary>
    public class MusicManager
    {
        
        private static readonly string MusicPath = "music";
        private static readonly string OnlineMusicFile = "online.ini";
        private static readonly string[] SupportedFormats = new string[] { ".mp3", ".wav", ".ogg" };

        private MusicMode mode;
        private Dictionary<string, List<string>> onlineTracks;

        public MusicManager(MusicMode mode)
        {
            this.mode = mode;

            if (mode == MusicMode.Offline)
            {
                if (!Directory.Exists(MusicManager.MusicPath))
                {
                    Directory.CreateDirectory(MusicManager.MusicPath);
                }
            }
            else if (mode == MusicMode.Online)
            {
                if (!File.Exists(MusicManager.OnlineMusicFile))
                {
                    File.Create(MusicManager.OnlineMusicFile);
                }

                this.onlineTracks = new Dictionary<string, List<string>>();
                this.ProcessOnlineIni();

            }
        }

        private void ProcessOnlineIni()
        {
            using (StreamReader reader = new StreamReader(MusicManager.OnlineMusicFile))
            {

                string currentMap = string.Empty;
                string line = string.Empty;

                do
                {
                    line = reader.ReadLine();

                    // TODO: Use regex instead
                    if (line.Trim().StartsWith("["))
                    {
                        currentMap = line.Replace("[", "").Replace("]", "").Trim();
                        this.onlineTracks[currentMap] = new List<string>();
                        System.Diagnostics.Debug.WriteLine("New map defined: " + currentMap);
                    }
                    else if (line.Trim() != string.Empty)
                    {
                        this.onlineTracks[currentMap].Add(line.Trim());
                        System.Diagnostics.Debug.WriteLine("Added track to " + currentMap + ":" + line.Trim());
                    }

                } while (!reader.EndOfStream);
            }
        }

        public List<string> GetTracks(string mapName)
        {
            List<string> result = new List<string>();

            if (this.mode == MusicMode.Offline)
            {
                result.AddRange(this.GetOfflineTracks(mapName));
            }
            else if (this.mode == MusicMode.Online)
            {
                if(this.onlineTracks.ContainsKey(mapName)){
                    result.AddRange(this.onlineTracks[mapName]);
                }
            }

            return result;
        }

        private List<string> GetOfflineTracks(string mapName)
        {
            List<string> result = new List<string>();
            
            // TODO:
            // If this map's name is not in our lookup table, we use the convention mapXXX
            // where XXX is the ID
            // string path = MusicManager.MusicPath + "\\map" + mapID;
            // currentMap = "map" + mapID;

            string path = MusicManager.MusicPath + "\\" + mapName;

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(Path.GetFullPath(path));
                foreach (string f in files)
                {
                    foreach (string format in SupportedFormats)
                    {
                        if (f.EndsWith(format))
                        {
                            result.Add(f);
                            break;
                        }
                    }
                }
            } // eo if (Directory.Exists(path))

            return result;
        }

    }
}
