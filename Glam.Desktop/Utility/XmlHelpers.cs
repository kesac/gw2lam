using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Glam.Desktop.Utility
{
    public static class XmlHelpers
    {
        public static bool Is(this XElement e, string name)
        {
            return e.Name == name;
        }

        public static string Get(this XElement e, string attribute)
        {
            return e.Attribute(attribute).Value;
        }
    }
}
