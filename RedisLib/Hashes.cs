using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet.RedisLib
{ 

    public class RedisHashes : RedisObject 
    {    
        public RedisHashes(RedisAsyncConnManager conn) : base (conn)
        {        
        }
        public RedisHashes(RedisAsyncConnManager conn, RedisClusterSupport rcs) : base (conn, rcs)
        {                                
        }
        public REDIS_RESPONSE_TYPE hdel(string key, string field, string value)
        {            
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("HDEL");
            m.Add(key);
            m.Add(field);
            
            if (value.Length != 0) m.Add(value);
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hexists(String key, String field)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);            

            m.Add("HEXISTS");
            m.Add(key);
            m.Add(field);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hget(String key, String field)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("HGET");
            m.Add(key);
            m.Add(field);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE hgetall(string hashname)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(hashname);
            
            m.Add("HGETALL");
            m.Add(hashname);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hincrby(String key, String field, int increment)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);
            
            m.Add("HINCRBY");
            m.Add(key);
            m.Add(field);
            m.Add(increment.ToString());

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hincrbyfloat(String key, String field, float increment)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("HINCRBYFLOAT");
            m.Add(key);
            m.Add(field);
            m.Add(increment.ToString());

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hkeys(String key)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("HKEYS");
            m.Add(key);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hlen(String key)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("HLEN");
            m.Add(key);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hmget(String key, params String[] fields)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("HMGET");
            m.Add(key);

            foreach( String s in fields )
            {                
                m.Add(s);
            }
            return Process(m);
        }        
        public REDIS_RESPONSE_TYPE hmset(String key, Dictionary<String, String> fields)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("HMSET");
            m.Add(key);

            foreach( KeyValuePair<String, String> kv in fields)
            {
                m.Add(kv.Key);
                m.Add(kv.Value);
            }
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hset(string hashname, string key, string value)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(hashname);
            m.Add("HSET");
            m.Add(hashname);
            m.Add(key);
            m.Add(value);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE hsetnx(String key, String field, String value)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("HSETNX");
            m.Add(key);
            m.Add(field);
            m.Add(value);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE hstrlen(String key, String field)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("HSTRLEN");
            m.Add(key);
            m.Add(field);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE hvals(String key)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);
            
            m.Add("HVALS");
            m.Add(key);

            return Process(m);            
        }
        public REDIS_RESPONSE_TYPE hscan(String key, String cursor, String pattern = null, String count = null)
        {
            RESPMaker m = new RESPMaker();
            AdjustClusterServer(key);

            m.Add("SCAN");
            m.Add(key);
            m.Add(cursor);

            if (!String.IsNullOrEmpty(pattern)) m.Add(pattern);
            if (!String.IsNullOrEmpty(count)) m.Add(count);
            return Process(m);
        }
    }
}