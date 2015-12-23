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
            List<String> list = new List<String>();
            Redis r = new Redis("192.168.184.217", 6379);
            RedisConnection c = r.GetConnection();
            c.select(3);

            RedisLists l = r.GetLists();

            l.lpush("listkey01", "100", "101", "102", "103");
            l.lpop("listkey01");
            l.getLists(ref list);
            
            


            

            


            /*Dictionary<string, string> dic = new Dictionary<string, string>();
            List<String> list = new List<String>();
            
            
            Redis r = new Redis("192.168.184.217", 6379);
            RedisConnection c = r.GetConnection();
            c.select(2);

            
            RedisHashes h = r.GetHashes();
            for (int i = 0; i < 100000; i++ )
            {
                h.hsetnx("test" + i.ToString(), "key" + i.ToString(), "val" + i.ToString());
            }
            RedisKeys k = r.GetKeys();

            k.scan("0");
            */

            
        }
    }
}
