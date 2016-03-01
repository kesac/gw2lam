using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glam
{
    public enum MusicType {
        WAV, 
        MP3, 
        OGG, 
        STREAM,
        OTHER
    }

    public class Music
    {
        public MusicType Type { get; set; }
        public string Path { get; set; }

        public Music(MusicType type, string path)
        {
            this.Type = type;
            this.Path = path;
        }
    }

    public interface MusicProvider
    {
        public abstract List<Music> GetMapMusic(uint mapId);
        public abstract List<Music> GetMapMusic(string mapName);
    }
}
