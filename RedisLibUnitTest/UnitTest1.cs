
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syncnet.RedisLib;

namespace RedisLibUnitTest
{
    [TestClass]
    public class RedisLibUnitTester
    {
        [TestCategory("Connection Object Test")]
        [TestMethod]
        public void ConnectionTest()
        {
            Redis r = new Redis("192.168.184.217", 6379);
            RedisConnection rc = r.GetConnection();

            rc.echo("Hello");

            Assert.AreEqual("Hello", rc.getString());

            rc.echo("World");

            Assert.AreEqual("World", rc.getString());


            
        }
        [TestCategory("Hashes Test")]
        [TestMethod]
        public void HashTest()
        {
            Dictionary<string, string> dic = new Dictionary<string,string>();
            Redis r = new Redis("192.168.184.217", 6379);
            RedisConnection c = r.GetConnection();
            c.select(10);

            RedisHashes h = r.GetHashes();

            h.hget("key00001", "field1");
            h.getDictionary(ref dic);

            Assert.AreEqual(0, dic.Count);
            

            
        }
    }
}
