using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Glam;
using System.Xml;
using System.Xml.Linq;

namespace Glam.Desktop
{
    public class MusicProvider : IMusicProvider
    {
        public string RootMusicFolder { get; private set; }
        private readonly string[] SupportedFormats = new string[] { "mp3", "wav", "ogg", "m3u" };

        private Dictionary<string, string> SpecialFolders;
        private string FallbackFolder;

        public MusicProvider(string musicFolderPath)
        {
            this.SpecialFolders = new Dictionary<string, string>();
            this.RootMusicFolder = musicFolderPath;

            if (!Directory.Exists(musicFolderPath))
            {
                Directory.CreateDirectory(musicFolderPath);   
            }

            try {
                if (File.Exists(this.RootMusicFolder + "\\music.xml"))
                {

                    XDocument x = XDocument.Load(this.RootMusicFolder + "\\music.xml");

                    foreach (XElement section in x.Descendants())
                    {
                        if (section.Name == "region")
                        {
                            string folder = section.Attribute("folder").Value;

                            foreach (XElement map in section.Descendants())
                            {
                                string mapName = map.Attribute("name").Value;

                                if (!this.SpecialFolders.Keys.Contains(mapName))
                                {
                                    this.SpecialFolders.Add(mapName, folder);
                                }

                            }
                        }
                        else if (section.Name == "fallback")
                        {
                            this.FallbackFolder = section.Attribute("folder").Value;
                        }
                    }

                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
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

            if (File.Exists(path + ".m3u")) // Check if a playlist exists for the map
            {
                Playlist playlist = new Playlist(path + ".m3u");
                result.AddRange(playlist.GetMusic());
            }

            if (Directory.Exists(path))
            {
                result.AddRange(this.GetMusicFromFolder(path));
            }

            if (this.SpecialFolders.Keys.Contains(mapName))
            {
                result.AddRange(this.GetMusicFromFolder(this.RootMusicFolder + "\\" + this.SpecialFolders[mapName]));
            }
            else if (result.Count == 0 && !string.IsNullOrEmpty(this.FallbackFolder))
            {
                result.AddRange(this.GetMusicFromFolder(this.RootMusicFolder + "\\" + this.FallbackFolder));
            }

            return result;
        }

        private List<Music> GetMusicFromFolder(string folder)
        {
            List<Music> result = new List<Music>();
            if (Directory.Exists(folder)) // Else check if a folder of files exists for the map
            {
                string[] files = Directory.GetFiles(Path.GetFullPath(folder));
                foreach (string f in files)
                {
                    foreach (string format in SupportedFormats)
                    {
                        if (f.ToLower().EndsWith("." + format))
                        {
                            if (format == "m3u")
                            {
                                Playlist playlist = new Playlist(f);
                                result.AddRange(playlist.GetMusic());
                            }
                            else {
                                Music m = new Music(f);

                                // Remove file extension from name
                                if(m.Name.EndsWith("." + format))
                                {
                                    m.Name = m.Name.Substring(0, m.Name.Length - ("." + format).Length);
                                }

                                result.Add(m);
                            }

                            break;
                        }
                    }
                }
            }
            return result;
        }

    }
}
