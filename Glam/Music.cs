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
        public string Name { get; set; }

        public Music(string path)
        {
            this.Path = path;

            if (!string.IsNullOrEmpty(this.Path))
            {
                string[] tokens = this.Path.Split('\\');
                this.Name = tokens[tokens.Length - 1];
            }
            else
            {
                this.Name = string.Empty;
            }
        }
    }
}
