using System;
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


            
        }
    }
}
