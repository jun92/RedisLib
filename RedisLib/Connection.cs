using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{
namespace RedisLib
{
    public class RedisConnection
    {
        public int retVal;
        private RedisConnManager conn;
        private RedisRESP2Class _rr; 

        public string getString()
        {
            return _rr.result_string;
        }
        public RedisConnection(RedisConnManager conn)
        {
            this.conn = conn;
            _rr = new RedisRESP2Class();
        }

        private REDIS_RESPONSE_TYPE Process(RESPMaker m)
        {
            _rr.parse(conn.Request(m));
            return _rr.response_type;
        }

        public REDIS_RESPONSE_TYPE auth(string password)
        {
            RESPMaker m         = new RESPMaker();
            RESPToken Command   = new RESPToken("AUTH");
            RESPToken pwd       = new RESPToken(password);          

            m.Add(Command);
            m.Add(pwd);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE echo(string message)
        {
            RESPMaker m     = new RESPMaker();
            RESPToken cmd   = new RESPToken("ECHO");
            RESPToken msg   = new RESPToken(message);

            m.Add(cmd);
            m.Add(msg);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE ping(string ping)
        {
            RESPMaker m = new RESPMaker();
            RESPToken cmd = new RESPToken("PING");

            m.Add(cmd);                        
            if( ping.Length != 0 )
            {
                RESPToken msg = new RESPToken(ping);
                m.Add(msg);
            }

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE quit()
        {
            RESPMaker m = new RESPMaker();
            RESPToken cmd = new RESPToken("QUIT");

            m.Add(cmd);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE select(int number)
        {
            RESPMaker m = new RESPMaker();
            RESPToken cmd = new RESPToken("SELECT");
            RESPToken n = new RESPToken(number);

            m.Add(cmd);
            m.Add(n);

            return Process(m);
        }
    }
}
}