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
            List<dynamic> narray = new List<dynamic>();
            Redis r = new Redis("192.168.184.217", 6379);
            RedisConnection c = r.GetConnection();
            c.select(2);

            RedisKeys k = r.GetKeys();

            k.scan("0");
            k.getNestedArray( ref narray);

            


            

            


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
