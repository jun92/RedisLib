
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
        /*
        [TestCategory("Scan Test")]
        [TestMethod]
        public void ScanTest()
        {
            List<dynamic> narray = new List<dynamic>();
            Redis r = new Redis("192.168.184.217", 6379);
            RedisConnection c = r.GetConnection();
            c.select(10);

            RedisKeys k = r.GetKeys();

            //k.scan("2");
            //k.getNestedArray(ref narray);
            //int count = (int)narray[0];
            //Assert.AreEqual(57344, count);
            //Assert.AreEqual(11, (List<String>)narray[1].Count);
        }
         */
        [TestCategory("List test")]
        [TestMethod]
        public void ListTest()
        {
            List<String> list = new List<String>();
            Redis r = new Redis("192.168.184.217", 6379);
            RedisConnection c = r.GetConnection();
            c.select(3);

            RedisLists l = r.GetLists();

            l.lpush("listkey01", "100", "101", "102", "103");
            l.lpop("listkey01");
            l.getLists(ref list);

            Assert.AreEqual("103", list[0].ToString());
            
            list.Clear();

            l.rpush("listrkey01", "200", "202");
            l.rpop("listrkey01");
            l.getLists(ref list);

            Assert.AreEqual("202", list[0].ToString());

        }
    }
}
