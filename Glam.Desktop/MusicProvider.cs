using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Glam;
using Glam.Desktop.Utility;


namespace Glam.Desktop
{
    public class MusicProvider : IMusicProvider
    {
        private readonly string MusicXmlFile = "music.xml";
        private readonly string[] SupportedFormats = new string[] { "mp3", "wav", "ogg", "m3u" };

        // These are XML node names and the attributes used for each node
        private readonly string LocalMusicNode = "folder";
        private readonly string LocalMusicNodeName = "name";
        private readonly string LocalMusicNodeType = "type";
        private readonly string MapGroupNode = "region";
        private readonly string MapGroupName = "name";
        private readonly string MapNode = "map";
        private readonly string MapNodeName = "name";
        private readonly string TrackNode = "track";
        private readonly string TrackNodeFile = "file";
        private readonly string EmbedNode = "embed";
        private readonly string EmbedNodeId = "id";
        
        private Dictionary<string, string> LocalMusicFolders;
        private string LocalMusicFallbackFolder;

        public string MusicFolder { get; private set; }

        public MusicProvider(string musicFolder)
        {
            this.MusicFolder = musicFolder;
            this.Reinitialize();
        }

        public bool Reinitialize()
        {
            try
            {
                this.LocalMusicFolders = new Dictionary<string, string>();

                if (!Directory.Exists(this.MusicFolder))
                {
                    Directory.CreateDirectory(this.MusicFolder);
                }

                string filename = this.MusicFolder + Path.DirectorySeparatorChar + MusicXmlFile;

                if (File.Exists(filename))
                {
                    var xml = XDocument.Load(filename);

                    foreach (var node in xml.Descendants())
                    {
                        // First check for local music folders (the tracks defined are on disk)
                        if (node.Is(LocalMusicNode))
                        {
                            
                            var type = node.Get(LocalMusicNodeType);

                            // Is it a fallback folder for maps without explicitly defined tracks?
                            if (type != null && type.ToLower() == "fallback")
                            {
                                this.LocalMusicFallbackFolder = node.Get(LocalMusicNodeName);
                            }
                            else
                            {
                                string folderName = node.Get(LocalMusicNodeName);

                                // Each child node is expected to be a declaration of a GW2 map
                                foreach (var childNode in node.Descendants())
                                {
                                    if (childNode.Is(MapNode))
                                    {
                                        string mapName = childNode.Get(MapNodeName);

                                        if (!this.LocalMusicFolders.Keys.Contains(mapName))
                                        {
                                            this.LocalMusicFolders.Add(mapName, folderName);
                                        }
                                    }

                                }
                            }
                        }
                        else if (node.Is(MapGroupNode))
                        {
                            string groupName = node.Get(MapGroupName);
                            foreach (var childNode in node.Descendants())
                            {
                                if (childNode.Is(MapNode))
                                {
                                    string mapName = childNode.Get(MapNodeName);

                                    // Each child node is expected to be a <track> or <embed>
                                    foreach (var musicNode in childNode.Descendants())
                                    {
                                        if (musicNode.Is(TrackNode))
                                        {
                                            string fileLocation = musicNode.Get(TrackNodeFile);
                                            // TODO: Add this to map's music queue
                                        }
                                        else if (musicNode.Is(EmbedNode))
                                        {
                                            string id = musicNode.Get(EmbedNodeId);
                                            // TODO: Add this to map's music queue
                                        }

                                    }
                                }
                            }
                        }

                    } // foreach node

                }

                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
                return false;
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

            string path = this.MusicFolder + "\\" + mapName;

            if (File.Exists(path + ".m3u")) // Check if a playlist exists for the map
            {
                Playlist playlist = new Playlist(path + ".m3u");
                result.AddRange(playlist.GetMusic());
            }

            if (Directory.Exists(path))
            {
                result.AddRange(this.GetMusicFromFolder(path));
            }

            if (this.LocalMusicFolders.Keys.Contains(mapName))
            {
                result.AddRange(this.GetMusicFromFolder(this.MusicFolder + "\\" + this.LocalMusicFolders[mapName]));
            }
            else if (result.Count == 0 && !string.IsNullOrEmpty(this.LocalMusicFallbackFolder))
            {
                result.AddRange(this.GetMusicFromFolder(this.MusicFolder + "\\" + this.LocalMusicFallbackFolder));
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
