using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syncnet.RedisLib;

namespace RedisLibUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Redis r = new Redis("192.168.184.217", 6379);
            RedisLists l = r.GetLists();
            l.rpush("Names", "Junsu", "sunghoon", "junga");
        }
    }
}
