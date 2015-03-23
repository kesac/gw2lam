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
        private static readonly string OnlineMusicFile = MusicPath + "\\online.ini";
        private static readonly string[] SUPPORTED_FORMATS = new string[] { ".mp3", ".wav", ".ogg" };

        private MusicMode mode;

        public MusicManager(MusicMode mode)
        {
            this.mode = mode;

            if (mode == MusicMode.Online)
            {
                if (!Directory.Exists(MusicManager.MusicPath))
                {
                    Directory.CreateDirectory(MusicManager.MusicPath);
                }

                if (!File.Exists(MusicManager.OnlineMusicFile))
                {
                    File.Create(MusicManager.OnlineMusicFile);
                }
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
                // read online.ini
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
                    foreach (string format in SUPPORTED_FORMATS)
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

        public void InitializeOnlineIni()
        {
            if (this.mode == MusicMode.Online)
            {
                
            }
        }

    }
}
