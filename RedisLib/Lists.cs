using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisLists
    {
        public int retVal;
        public RedisConnManager conn;
        public RedisRESP2Class _rr; 
        public RedisLists(RedisConnManager conn)
        {
            this.conn = conn;
            _rr = new RedisRESP2Class();

        }
        public REDIS_RESPONSE_TYPE rpush(String key, params string[] values )
        {
            RESPToken resp_command  = new RESPToken("RPUSH");
            RESPToken resp_key      = new RESPToken(key);

            RESPMaker m = new RESPMaker();

            m.Add(resp_command);
            m.Add(resp_key);

            foreach( string e in values)
            {
                RESPToken resp_value = new RESPToken(e);
                m.Add(resp_value);
            }
            _rr.parse(conn.Request(m));
            return _rr.response_type;
        }
        public REDIS_RESPONSE_TYPE lrange(String key, String start, String end)
        {
            RESPToken resp_command  = new RESPToken("LRANGE");
            RESPToken resp_key      = new RESPToken(key);
            RESPToken resp_start    = new RESPToken(start);
            RESPToken resp_end      = new RESPToken(end);

            RESPMaker m = new RESPMaker();

            m.Add(resp_command);
            m.Add(resp_key);
            m.Add(resp_start);
            m.Add(resp_end);
            _rr.parse(conn.Request(m));

            return _rr.response_type;
        }
    }
}
}
