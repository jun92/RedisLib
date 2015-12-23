using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{
namespace RedisLib
{
    public class RedisSets : RedisObject
    {
        public RedisSets(RedisConnManager conn) : base(conn)
        {
        }
        public REDIS_RESPONSE_TYPE sadd(String key, String member, params String[] members)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SADD");
            m.Add(key);
            m.Add(member);
            foreach(String s in members)
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE scard(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SCARD");
            m.Add(key);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE sdiff(String key, params String[] keys)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SDIFF");
            m.Add(key);
            foreach(String s in keys)
            {
                m.Add(s);
            }
            return Process(m);            
        }

        public REDIS_RESPONSE_TYPE sdiffstore(String dest, String key, params String[] keys)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SDIFFSTORE");
            m.Add(dest);
            m.Add(key);
            foreach( String s in keys)
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE sinter(String key, params String[] keys)
        {
            RESPMaker m = new RESPMaker();

            m.Add("SINTER");
            m.Add(key);
            foreach( String s in keys )
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE sinterstore(String dest, String key, params String[] keys)
        {
            RESPMaker m = new RESPMaker();

            m.Add("SINTERSTORE");
            m.Add(dest);
            m.Add(key);
            foreach( String s in keys )
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE sismember(String key, String member)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SISMEMBER");
            m.Add(key);
            m.Add(member);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE smembers(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SMEMBERS");
            m.Add(key);
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE smove(String src, String dest, String member)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SMOVE");
            m.Add(src);
            m.Add(dest);
            m.Add(member);
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE spop(String key, int count = 0)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SPOP");
            m.Add(key);
            if( count != 0 )
            {
                m.Add(count);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE srandmember(String key, int count = 0)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SRANDMEMBER");
            m.Add(key);
            if (count != 0)
            {
                m.Add(count);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE srem(String key, String member, params String[] members)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SREM");
            m.Add(key);
            m.Add(member);
            foreach(String s in members)
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE sscan(String key, String cursor, String pattern = null , String count = null )
        {
            RESPMaker m = new RESPMaker();

            m.Add("SSCAN");
            m.Add(key);
            m.Add(cursor);

            if (!String.IsNullOrEmpty(pattern)) m.Add(pattern);
            if (!String.IsNullOrEmpty(count)) m.Add(count);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE sunion(String key, params String[] keys)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SUION");
            m.Add(key);
            foreach( String s in keys )
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE sunionstore(String dest, String key, params String[] keys)
        {
            RESPMaker m = new RESPMaker();
            m.Add("SUIONSTORE");
            m.Add(dest);
            m.Add(key);
            foreach (String s in keys)
            {
                m.Add(s);
            }
            return Process(m);
        }
    }
}
}