using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncnet.RedisLib;

namespace TestApp
{
    class TestMain
    {
        static void Main(string[] args)
        {
            Redis r = new Redis("192.168.184.217", 6379);

            RedisHashes h = r.GetHashes();
            h.hset("aaaa", "kekekekek", "lalalala");
            h.hgetall("aaaa");
            Dictionary<String, String> dd = new Dictionary<string,string>();
            r._conn.rr.getAsDictionary(ref dd);

            Console.WriteLine("{0}", dd["kekekekek"].ToString());
            //RedisLists l = r.GetLists();
            //l.lrange("Names", "-3", "2");
            //l.rpush("Names", "Junsu", "sunghoon", "junga");
        }
    }
}
