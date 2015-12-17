using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet 
{
namespace RedisLib
{
    public class RedisStrings
    {
        public int retVal;
        public RedisConnManager conn;
        public RedisRESP2Class _rr;
        public RedisStrings(RedisConnManager conn)
        {
            this.conn = conn;
            _rr = new RedisRESP2Class();

        }
        public REDIS_RESPONSE_TYPE Append(string key, string value)
        {
            
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_value = new RESPToken(value);

            RESPToken Command = new RESPToken("APPEND");

            RESPMaker m = new RESPMaker();

            m.Add(Command);
            m.Add(resp_key);
            m.Add(resp_value);

            _rr.parse(conn.Request(m));
            return _rr.response_type;
        }
    }
}
}
