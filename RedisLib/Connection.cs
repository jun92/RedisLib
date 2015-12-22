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

        public RedisConnection(RedisConnManager conn) : base( conn) 
        { 
        }
        public REDIS_RESPONSE_TYPE auth(string password)
        {
            RESPMaker m         = new RESPMaker();           

            m.Add(new RESPToken("AUTH"));
            m.Add(new RESPToken(password));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE echo(string message)
        {
            RESPMaker m     = new RESPMaker();

            m.Add(new RESPToken("ECHO"));
            m.Add(new RESPToken(message));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE ping(string ping)
        {
            RESPMaker m = new RESPMaker();            

            m.Add(new RESPToken("PING"));
            if( ping.Length != 0 ) m.Add(new RESPToken(ping));            

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE quit()
        {
            RESPMaker m = new RESPMaker();            

            m.Add(new RESPToken("QUIT"));

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE select(int number)
        {
            RESPMaker m = new RESPMaker();            

            m.Add(new RESPToken("SELECT"));
            m.Add(new RESPToken(number.ToString()));

            return Process(m);
        }
    }
}
}