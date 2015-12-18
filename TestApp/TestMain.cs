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
            List<String> list = new List<String>();
            
            
            Redis r = new Redis("192.168.184.217", 6379);
            RedisConnection c = r.GetConnection();
            c.select(1);

            RedisHashes h = r.GetHashes();

            h.hkeys("key0001");
            h.getLists(ref list);

            
            
            

            
        }
    }
}
