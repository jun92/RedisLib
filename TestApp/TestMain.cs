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


            
            RedisConnection rc = r.GetConnection();

            rc.echo("Hello");
            
            

            
        }
    }
}
