using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Glam;

namespace Glam.Desktop
{
    // Only handles .m3u files like GW2 does
    public class Playlist
    {
        private string FilePath;

        public Playlist(string filePath)
        {
            this.FilePath = filePath;
        }

        public List<Music> GetMusic()
        {

            List<Music> results = new List<Music>();

            if (File.Exists(this.FilePath))
            {

                using (StreamReader reader = new StreamReader(this.FilePath))
                {
                    string buffer = string.Empty;
                    while (!reader.EndOfStream)
                    {
                        buffer = reader.ReadLine();

                        if (!buffer.StartsWith("#") && buffer.Trim().Length > 0)
                        {
                            string filePath = buffer.Trim();

                            // If the filename has non-ASCII characters, it will show up as "?"
                            // characters inside the .m3u file. 
                            if (File.Exists(filePath)) { 
                                results.Add(new Music(buffer.Trim()));
                            }

                        }
                    }
                }

            }

            return results;

        }

    }
}
