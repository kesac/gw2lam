using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turtlesort.Glam.Core;

namespace gw2lam_tests
{
    [TestClass]
    public class MapsApiTest
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

    }
}
