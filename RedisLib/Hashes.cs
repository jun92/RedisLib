using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisHashes
    {
        public int retVal;
        public RedisConnManager conn;
        public RedisHashes(RedisConnManager conn)
        {
            this.conn = conn;

        }
        public REDIS_RESPONSE_TYPE hset(string hashname, string key, string value)
        {
            REDIS_RESPONSE_TYPE res;
            RESPToken resp_hash = new RESPToken(hashname);
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_value = new RESPToken(value);

            RESPToken Command = new RESPToken("HSET");

            RESPMaker m = new RESPMaker();

            m.Add(Command);
            m.Add(resp_hash);
            m.Add(resp_key);
            m.Add(resp_value);

            res = conn.Request(m);
            return res;
        }
        public REDIS_RESPONSE_TYPE hgetall(string hashname)
        {
            RESPMaker m = new RESPMaker();

            RESPToken resp_hashname = new RESPToken(hashname);
            RESPToken resp_command = new RESPToken("HGETALL");

            m.Add(resp_command);
            m.Add(resp_hashname);

            return conn.Request(m);
        }

    }
}
}