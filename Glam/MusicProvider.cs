using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glam
{
    public class Music
    {
        public string Path { get; set; }
        public string Tags { get; set; }

        public Music(string path)
        {
            this.Path = path;
        }
    }

    public interface MusicProvider
    {
        List<Music> GetMapMusic(uint mapId);
        List<Music> GetMapMusic(string mapName);
    }
}
