using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Glam;

namespace Glam.Desktop
{
    public class DesktopMusicProvider : MusicProvider
    {
        private string RootMusicFolder;
        private readonly string[] SupportedFormats = new string[] { ".mp3", ".wav", ".ogg" };

        public DesktopMusicProvider(string musicFolderPath)
        {
            this.RootMusicFolder = musicFolderPath;

            if (!Directory.Exists(musicFolderPath))
            {
                Directory.CreateDirectory(musicFolderPath);   
            }
        }

        public List<Music> GetMapMusic(uint mapId)
        {
            // If this map's name is not in our lookup table, we use the convention mapXXX
            // where XXX is the ID
            return this.GetMusic("map" + mapId.ToString());
        }

        public List<Music> GetMapMusic(string mapName)
        {
            return this.GetMusic(mapName);
        }

        private List<Music> GetMusic(string mapName)
        {
            List<Music> result = new List<Music>();

            string path = this.RootMusicFolder + "\\" + mapName;

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(Path.GetFullPath(path));
                foreach (string f in files)
                {
                    foreach (string format in SupportedFormats)
                    {
                        if (f.EndsWith(format))
                        {
                            result.Add(new Music(f));
                            break;
                        }
                    }
                }
            }

            return result;
        }

    }
}
