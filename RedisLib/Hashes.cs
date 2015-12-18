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
            RESPToken cmd = new RESPToken("HDEL");
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_field = new RESPToken(field);

            m.Add(cmd);
            m.Add(resp_key);
            m.Add(resp_field);
            
            if (value.Length != 0)
            {
                RESPToken resp_value = new RESPToken(value);
                m.Add(resp_value);
            }
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hexists(String key, String field)
        {
            RESPMaker m = new RESPMaker();
            RESPToken cmd = new RESPToken("HEXISTS");
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_field = new RESPToken(field);

            m.Add(cmd);
            m.Add(resp_key);
            m.Add(resp_field);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hget(String key, String field)
        {
            RESPMaker m = new RESPMaker();
            RESPToken cmd = new RESPToken("HGET");
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_field = new RESPToken(field);

            m.Add(cmd);
            m.Add(resp_key);
            m.Add(resp_field);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE hgetall(string hashname)
        {
            RESPMaker m = new RESPMaker();
            RESPToken resp_command = new RESPToken("HGETALL");
            RESPToken resp_hashname = new RESPToken(hashname);

            m.Add(resp_command);
            m.Add(resp_hashname);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hincrby(String key, String field, int increment)
        {
            RESPMaker m = new RESPMaker();
            RESPToken resp_cmd = new RESPToken("HINCRBY");
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_field = new RESPToken(field);
            RESPToken resp_incre = new RESPToken(increment.ToString());

            m.Add(resp_cmd);
            m.Add(resp_key);
            m.Add(resp_field);
            m.Add(resp_incre);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hincrbyfloat(String key, String field, float increment)
        {
            RESPMaker m = new RESPMaker();
            RESPToken resp_cmd = new RESPToken("HINCRBYFLOAT");
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_field = new RESPToken(field);
            RESPToken resp_incre = new RESPToken(increment.ToString());

            m.Add(resp_cmd);
            m.Add(resp_key);
            m.Add(resp_field);
            m.Add(resp_incre);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hkeys(String key)
        {
            RESPMaker m = new RESPMaker();
            RESPToken resp_cmd = new RESPToken("HKEYS");
            RESPToken resp_key = new RESPToken(key);

            m.Add(resp_cmd);
            m.Add(resp_key);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hlen(String key)
        {
            RESPMaker m = new RESPMaker();
            RESPToken resp_cmd = new RESPToken("HLEN");
            RESPToken resp_key = new RESPToken(key);

            m.Add(resp_cmd);
            m.Add(resp_key);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hmget(String key, params String[] fields)
        {
            RESPMaker m = new RESPMaker();
            RESPToken resp_cmd = new RESPToken("HMGET");
            RESPToken resp_key = new RESPToken(key);

            m.Add( resp_cmd );
            m.Add( resp_key );

            foreach( String s in fields )
            {
                RESPToken field = new RESPToken(s);
                m.Add(field);
            }
            return Process(m);
        }        
        public REDIS_RESPONSE_TYPE hmset(String key, Dictionary<String, String> fields)
        {
            RESPMaker m = new RESPMaker();
            RESPToken resp_key = new RESPToken(key);

            m.Add(resp_key);

            foreach( KeyValuePair<String, String> kv in fields)
            {
                RESPToken fKey = new RESPToken(kv.Key);
                RESPToken fVal = new RESPToken(kv.Value);

                m.Add(fKey);
                m.Add(fVal);
            }
            return Process(m);
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

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hsetnx(String key, String field, String value)
        {
            RESPMaker m = new RESPMaker();
            RESPToken Command = new RESPToken("HSETNX");
            RESPToken resp_hash = new RESPToken(key);
            RESPToken resp_key = new RESPToken(field);
            RESPToken resp_value = new RESPToken(value);

            m.Add(Command);
            m.Add(resp_hash);
            m.Add(resp_key);
            m.Add(resp_value);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE hstrlen(String key, String field)
        {
            RESPMaker m = new RESPMaker();

            RESPToken cmd = new RESPToken("HSTRLEN");
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_field = new RESPToken(field);

            m.Add(cmd);
            m.Add(resp_key);
            m.Add(resp_field);           

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE hvals(String key)
        {
            RESPMaker m = new RESPMaker();
            RESPToken cmd = new RESPToken("HVALS");
            RESPToken resp_key = new RESPToken(key);
            
            m.Add(cmd);
            m.Add(resp_key);

            return Process(m);            
        }
    }
}
}