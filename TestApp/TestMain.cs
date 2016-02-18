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
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<string> li = new List<String>();
            //REDIS_RESPONSE_TYPE ret;


            Redis r = new Redis("192.168.184.217", 7000, true);
            
            
            /*
            RedisConnection c = r.GetConnection();
            ret = c.select(10);
            if( ret == REDIS_RESPONSE_TYPE.ERROR)
            {
                Console.WriteLine("{0}", c.getError());
                return;
            }
            RedisLists l = r.GetLists();*/
            
            /*for (int i = 0; i < 10000; i++)
            {
                l.lpush("longlist", "list" + i.ToString());
            }*/
            /*
            l.lrange("longlist", 0, -1);
            l.getLists(ref li);
             */ 
            
            //RedisClusterSupport rcs = r.GetClusterSupport();

            /*RedisHashes h = r.GetHashes();
            h.hget("key00001", "field1");
            h.getDictionary(ref dic);*/

            //rcs.ConstructClusterConfigInfo();
           
            /*
            List<String> list = new List<String>();
            Redis r = new Redis("192.168.184.217", 6379);
            RedisConnection c = r.GetConnection();
            c.select(3);
            RedisLists l = r.GetLists();
            l.lpush("listkey01", "100", "101", "102", "103");
            l.lpop("listkey01");
            l.getLists(ref list);
            */
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
