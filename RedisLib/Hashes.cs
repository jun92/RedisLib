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
        public RedisRESP2Class _rr; 
        
        public RedisHashes(RedisConnManager conn)
        {
            this.conn = conn;
            _rr = new RedisRESP2Class();

        }
        public bool getDictionary(ref Dictionary<String,String> dic )
        {
            _rr.getAsDictionary(ref dic);
            return true;             
        }
        public REDIS_RESPONSE_TYPE hset(string hashname, string key, string value)
        {
            RESPMaker m = new RESPMaker();
            RESPToken Command       = new RESPToken("HSET");
            RESPToken resp_hash     = new RESPToken(hashname);
            RESPToken resp_key      = new RESPToken(key);
            RESPToken resp_value    = new RESPToken(value);         

            m.Add(Command);
            m.Add(resp_hash);
            m.Add(resp_key);
            m.Add(resp_value);
            _rr.parse(conn.Request(m));
            return _rr.response_type;
        }
        public REDIS_RESPONSE_TYPE hgetall(string hashname)
        {
            RESPMaker m = new RESPMaker();
            RESPToken resp_command      = new RESPToken("HGETALL");
            RESPToken resp_hashname     = new RESPToken(hashname);
            
            m.Add(resp_command);
            m.Add(resp_hashname);
            _rr.parse(conn.Request(m)); 
            return _rr.response_type;
        }
    }
}
}