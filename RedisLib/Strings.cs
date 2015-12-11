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
        public RedisStrings(RedisConnManager conn)
        {
            this.conn = conn;

        }
        public REDIS_RESPONSE_TYPE Append(string key, string value)
        {
            REDIS_RESPONSE_TYPE res;
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_value = new RESPToken(value);

            RESPToken Command = new RESPToken("APPEND");

            RESPMaker m = new RESPMaker();

            m.Add(Command);
            m.Add(resp_key);
            m.Add(resp_value);

            res = conn.Request(m);
            return res;
        }

    }
}
}
