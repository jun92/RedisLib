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
           
        public RedisLists(RedisAsyncConnManager conn) : base(conn)
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

            m.Add("BLPOP");
            foreach( String s in extra_keys)
            {
                m.Add(s);
            }
            m.Add(timeout.ToString());

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE brpop(String key, int timeout, params String[] extra_keys)
        {
            RESPMaker m = new RESPMaker();

            m.Add("BRPOP");
            foreach (String s in extra_keys)
            {
                m.Add(s);
            }
            m.Add(timeout.ToString());

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE brpoplpush(String src, String dest, int timeout )
        {
            RESPMaker m = new RESPMaker();
            
            m.Add("BRPOPLPUSH");
            m.Add(src);
            m.Add(dest);
            m.Add(timeout.ToString());

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lindex(String key, int index)
        {
            RESPMaker m = new RESPMaker();
            m.Add("LINDEX");
            m.Add(key);
            m.Add(index.ToString());
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE linsert(String key, bool IsBefore, String pivot, String value)
        {
            RESPMaker m = new RESPMaker();
            m.Add("LINSERT");
            m.Add(key);
            if (IsBefore) m.Add("BEFORE");
            else m.Add("AFTER");
            m.Add(pivot);
            m.Add(value);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE llen(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add("LLEN");
            m.Add(key);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lpop(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add("LPOP");
            m.Add(key);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lpush(String key, String value, params String[] values)
        {
            RESPMaker m = new RESPMaker();

            m.Add("LPUSH");
            m.Add(key);
            m.Add(value);

            foreach( String s in values )
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lpushx(String key, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add("LPUSH");
            m.Add(key);
            m.Add(value);
            
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lrange(String key, int start, int end)
        {
            RESPMaker m = new RESPMaker();

            m.Add("LRANGE");
            m.Add(key);
            m.Add(start.ToString());
            m.Add(end.ToString());

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lrem(String key, int count, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add("LREM");
            m.Add(key);
            m.Add(count.ToString());
            m.Add(value);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lset(String key, int index, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add("LSET");
            m.Add(key);
            m.Add(index.ToString());
            m.Add(value);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE ltrim(String key, int start, int stop)
        {
            RESPMaker m = new RESPMaker();

            m.Add("LTRIM");
            m.Add(key);
            m.Add(start.ToString());
            m.Add(stop.ToString());

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rpop(String key)
        {
            RESPMaker m = new RESPMaker();

            m.Add("RPOP");
            m.Add(key);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rpoplpush(String src, String dest)
        {
            RESPMaker m = new RESPMaker();

            m.Add("RPOPLPUSH");
            m.Add(src);
            m.Add(dest);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rpush(String key, String value,  params string[] values )
        {
            RESPMaker m = new RESPMaker();

            m.Add("RPUSH");
            m.Add(key);
            m.Add(value);

            foreach( string e in values)
            {                
                m.Add(e);
            }

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rpush(String key, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add("RPUSH");
            m.Add(key);
            m.Add(value);           

            return Process(m);
        }
    }
}
}
