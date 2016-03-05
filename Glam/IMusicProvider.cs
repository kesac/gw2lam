using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glam
{
    public interface IMusicProvider
    {
        List<Music> GetMapMusic(uint mapId);
        List<Music> GetMapMusic(string mapName);
    }
}
