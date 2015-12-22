using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{ 
namespace RedisLib
{
    /// <summary>
    /// 레디스의 list관련 기능을 제공하는 클래스 
    /// </summary>
    public class RedisLists : RedisObject 
    {
           
        public RedisLists(RedisConnManager conn) : base(conn)
        {
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <param name="extra_keys"></param>
        /// <returns></returns>
        /// <seealso cref="http://redis.io/commands/blpop"/>
        public REDIS_RESPONSE_TYPE blpop(String key, int timeout, params String[] extra_keys)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("BLPOP"));
            foreach( String s in extra_keys)
            {
                m.Add(new RESPToken(s));
            }
            m.Add(new RESPToken(timeout.ToString()));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE brpop(String key, int timeout, params String[] extra_keys)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("BRPOP"));
            foreach (String s in extra_keys)
            {
                m.Add(new RESPToken(s));
            }
            m.Add(new RESPToken(timeout.ToString()));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE brpoplpush(String src, String dest, int timeout )
        {
            RESPMaker m = new RESPMaker();
            
            m.Add(new RESPToken("BRPOPLPUSH"));
            m.Add(new RESPToken(src));
            m.Add(new RESPToken(dest));
            m.Add(new RESPToken(timeout.ToString()));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lindex(String key, int index)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("LINDEX"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(index.ToString()));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE linsert(String key, bool IsBefore, String pivot, String value)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("LINSERT"));
            m.Add(new RESPToken(key));
            if (IsBefore) m.Add(new RESPToken("BEFORE"));
            else m.Add(new RESPToken("AFTER"));
            m.Add(new RESPToken(pivot));
            m.Add(new RESPToken(value));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE llen(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("LLEN"));
            m.Add(new RESPToken(key));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lpop(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("LPOP"));
            m.Add(new RESPToken(key));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lpush(String key, String value, params String[] values)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("LPUSH"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));

            foreach( String s in values )
            {
                m.Add(new RESPToken(s));
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lpushx(String key, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("LPUSH"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));
            
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lrange(String key, String start, String end)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("LRANGE"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(start));
            m.Add(new RESPToken(end));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lrem(String key, int count, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("LREM"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(count.ToString()));
            m.Add(new RESPToken(value));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lset(String key, int index, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("LSET"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(index.ToString()));
            m.Add(new RESPToken(value));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE ltrim(String key, int start, int stop)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("LTRIM"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(start.ToString()));
            m.Add(new RESPToken(stop.ToString()));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rpop(String key)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("RPOP"));
            m.Add(new RESPToken(key));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rpoplpush(String src, String dest)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("RPOPLPUSH"));
            m.Add(new RESPToken(src));
            m.Add(new RESPToken(dest));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rpush(String key, String value,  params string[] values )
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("RPUSH"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));

            foreach( string e in values)
            {                
                m.Add(new RESPToken(e));
            }

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rpush(String key, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("RPUSH"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));           

            return Process(m);
        }
    }
}
}
