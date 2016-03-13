using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glam.Desktop
{
    public class VisualsProvider
    {

        private readonly string[] SupportedFormats = new string[] { "png", "jpg" };

        public string VisualsFolder { get; set; }
        public List<string> Images { get; set; }

        public VisualsProvider(string folderPath)
        {
            
            this.VisualsFolder = folderPath;
            this.Images = new List<string>();

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            else
            {
                foreach(string file in Directory.GetFiles(folderPath))
                {
                    foreach(string format in SupportedFormats)
                    {
                        if (file.ToLower().EndsWith("." + format))
                        {
                            this.Images.Add(Path.GetFullPath(file));
                        }
                    }
                }
            }
        }

        public bool HasVisuals()
        {
            return this.Images != null && this.Images.Count > 0;
        }

        public string GetRandomImageFile()
        {
            if(this.HasVisuals())
            {
                Random r = new Random();
                return this.Images[r.Next(this.Images.Count)];
            }
            else
            {
                return null;
            }
        }
    }
}
