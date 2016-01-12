using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{
namespace RedisLib
{
    public class RedisConnection : RedisObject
    {

        public RedisConnection(RedisAsyncConnManager conn) : base( conn) 
        { 
        }
        public REDIS_RESPONSE_TYPE auth(string password)
        {
            RESPMaker m         = new RESPMaker();           

            m.Add("AUTH");
            m.Add(password);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE echo(string message)
        {
            RESPMaker m     = new RESPMaker();

            m.Add("ECHO");
            m.Add(message);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE ping(string ping)
        {
            RESPMaker m = new RESPMaker();            

            m.Add("PING");
            if( ping.Length != 0 ) m.Add(new RESPToken(ping));            

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE quit()
        {
            RESPMaker m = new RESPMaker();            

            m.Add("QUIT");

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE select(int number)
        {
            RESPMaker m = new RESPMaker();            

            m.Add("SELECT");
            m.Add(number.ToString());

            return Process(m);
        }
    }
}
}