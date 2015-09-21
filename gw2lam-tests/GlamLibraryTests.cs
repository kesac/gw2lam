using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turtlesort.Glam.Core;

namespace Turtlesort.Glam.Tests
{
    [TestClass]
    public class GlamLibraryTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            MapsApi api = new MapsApi();
            api.EnableLocalCache = true;
            Dictionary <uint, Map> maps = api.Read();
            
            System.Console.WriteLine(maps.Count);

            int limit = 3;
            foreach (Map m in maps.Values)
            {
                System.Console.WriteLine(m.Id + "\t" + m.Name);
                if (--limit == 0) break;
            }
        }


        [TestMethod]
        public void SettingsTestSave()
        {
            Settings settings = new Settings();

            settings.data["setting1"] = true;
            settings.data["setting2"] = 57;
            settings.data["setting3"] = "hello, world";

            settings.SaveToFile("savefile.txt");
        }

        [TestMethod]
        public void SettingsTestLoad()
        {
            Settings settings = new Settings();
            settings.LoadFromFile(@"E:\Projects\C#\gw2lam\trunk\gw2lam-tests\bin\Release\savefile.txt");

            System.Console.WriteLine("setting1: " + settings.data["setting1"]);
            System.Console.WriteLine("setting2: " + settings.data["setting2"]);
            System.Console.WriteLine("setting3: " + settings.data["setting3"]);

        }

    }
}
