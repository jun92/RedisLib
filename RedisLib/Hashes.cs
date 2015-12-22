using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisHashes : RedisObject 
    {        
        public RedisHashes(RedisConnManager conn) : base (conn)
        {
        }
        public REDIS_RESPONSE_TYPE hdel(string key, string field, string value)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HDEL"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(field));
            
            if (value.Length != 0) m.Add(new RESPToken(value));            
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hexists(String key, String field)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HEXISTS"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(field));

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hget(String key, String field)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HGET"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(field));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE hgetall(string hashname)
        {
            RESPMaker m = new RESPMaker();
            
            m.Add(new RESPToken("HGETALL"));
            m.Add(new RESPToken(hashname));

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hincrby(String key, String field, int increment)
        {
            RESPMaker m = new RESPMaker();
            
            m.Add(new RESPToken("HINCRBY"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(field));
            m.Add(new RESPToken(increment.ToString()));

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hincrbyfloat(String key, String field, float increment)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HINCRBYFLOAT"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(field));
            m.Add(new RESPToken(increment.ToString()));

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hkeys(String key)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HKEYS"));
            m.Add(new RESPToken(key));

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hlen(String key)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HLEN"));
            m.Add(new RESPToken(key));

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hmget(String key, params String[] fields)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HMGET"));
            m.Add(new RESPToken(key));

            foreach( String s in fields )
            {                
                m.Add(new RESPToken(s));
            }
            return Process(m);
        }        
        public REDIS_RESPONSE_TYPE hmset(String key, Dictionary<String, String> fields)
        {
            RESPMaker m = new RESPMaker();            

            m.Add(new RESPToken(key));

            foreach( KeyValuePair<String, String> kv in fields)
            {
                m.Add(new RESPToken(kv.Key));
                m.Add(new RESPToken(kv.Value));
            }
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hset(string hashname, string key, string value)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HSET"));
            m.Add(new RESPToken(hashname));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hsetnx(String key, String field, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HSETNX"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(field));
            m.Add(new RESPToken(value));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE hstrlen(String key, String field)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("HSTRLEN"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(field));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE hvals(String key)
        {
            RESPMaker m = new RESPMaker();
            
            m.Add(new RESPToken("HVALS"));
            m.Add(new RESPToken(key));

            return Process(m);            
        }
    }
}
}