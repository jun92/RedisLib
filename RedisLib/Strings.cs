using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet 
{
namespace RedisLib
{

    public class RedisStrings : RedisObject
    {
        public enum STRING_BITOP_TYPE
        {
            AND = 0,
            OR = 1,
            XOR = 2,
            NOT = 3
        }
        public enum STRING_SET_TYPE
        {
            NONE = 0,
            NX = 1,
            XX = 2
        }
        
        public RedisStrings(RedisAsyncConnManager conn) : base(conn)
        {            
        }
        public REDIS_RESPONSE_TYPE append(string key, string value)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("APPEND"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE bitcount(String key, int start = -1, int end = -1 )
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("BITCOUNT"));
            m.Add(new RESPToken(key));

            if( start != -1 && end != -1 )
            {
                m.Add(new RESPToken(start.ToString()));
                m.Add(new RESPToken(end.ToString()));
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE bitop(STRING_BITOP_TYPE op, String destkey, String key, params String[] keys)
        {            
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("BITOP"));

            switch( op )
            {
                case STRING_BITOP_TYPE.AND: m.Add("AND"); break;
                case STRING_BITOP_TYPE.NOT: m.Add("NOT"); break;
                case STRING_BITOP_TYPE.OR: m.Add("OR"); break;
                case STRING_BITOP_TYPE.XOR: m.Add("XOR"); break;
            }            
            m.Add(new RESPToken(destkey));
            m.Add(new RESPToken(key));
            foreach(String s in keys)
            {
                m.Add(new RESPToken(s));
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE bitpos(String key, String bit)
        {
            RESPMaker m = new RESPMaker();
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE decr(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("DECR"));
            m.Add(new RESPToken(key));
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE decrby(String key, int decrement)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("DECRBY"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(decrement.ToString()));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE get(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("GET"));
            m.Add(new RESPToken(key));            
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE getbit(String key, int offset)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("GETBIT"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(offset.ToString()));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE getrange(String key, int start,  int end)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("GETRANGE"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(start.ToString()));
            m.Add(new RESPToken(end.ToString()));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE getset(String key, String value)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("GETSET"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE incr(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("INCR"));
            m.Add(new RESPToken(key));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE incrby(String key, int increment)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("INCRBY"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(increment.ToString()));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE incrbyfloat(String key, float increment)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("INCRBYFLOAT"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(increment.ToString()));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE mget(String key, params String[] keys)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("MGET"));
            m.Add(new RESPToken(key));
            foreach(String s in keys)
            {
                m.Add(new RESPToken(s));
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE mset(String key, String value, params String[] keyvalues )
        {
            //caution: there is only way to verify the keyvalues by checking pairs or not. 
            //         if you mistake the order of keys and values, it will be so.
            //         it is a better way to use the other version of the method. 
            if ( (keyvalues.Length % 2) != 0) return REDIS_RESPONSE_TYPE.ERROR;

            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("MSET"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));
            
            foreach(String s in keyvalues)
            {
                m.Add(new RESPToken(s));            
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE mset(Dictionary<String, String> keyvalues)
        {
            if (keyvalues.Count == 0) return REDIS_RESPONSE_TYPE.ERROR;
            RESPMaker m = new RESPMaker();
            
            m.Add(new RESPToken("MSET"));
            foreach(KeyValuePair<String, String> s in keyvalues)
            {
                m.Add(new RESPToken(s.Key));
                m.Add(new RESPToken(s.Value));
            }
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE msetnx(String key, String value, params String[] keyvalues)
        {
            //caution: there is only way to verify the keyvalues by checking pairs or not. 
            //         if you mistake the order of keys and values, it will be so.
            //         it is a better way to use the other version of the method. 
            if ((keyvalues.Length % 2) != 0) return REDIS_RESPONSE_TYPE.ERROR;

            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("MSETNX"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));

            foreach (String s in keyvalues)
            {
                m.Add(new RESPToken(s));
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE msetnx(Dictionary<String, String> keyvalues)
        {
            if (keyvalues.Count == 0) return REDIS_RESPONSE_TYPE.ERROR;
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("MSETNX"));
            foreach (KeyValuePair<String, String> s in keyvalues)
            {
                m.Add(new RESPToken(s.Key));
                m.Add(new RESPToken(s.Value));
            }
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE psetex(String key, int millisec, String value)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("PSETEX"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(millisec.ToString()));
            m.Add(new RESPToken(value));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE set(String key, String value, int sec = 0, int millisec = 0, STRING_SET_TYPE IsExist = STRING_SET_TYPE.NONE)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("SET"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));
            if (sec != 0)
            {
                m.Add(new RESPToken("EX"));
                m.Add(new RESPToken(sec.ToString()));
            }
            if( millisec != 0 )
            {
                m.Add(new RESPToken("PX"));
                m.Add(new RESPToken(millisec.ToString()));
            }
            switch( IsExist )
            {
                case STRING_SET_TYPE.NX: m.Add("NX"); break;
                case STRING_SET_TYPE.XX: m.Add("XX"); break;
            }            
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE setbit(String key, int offset, bool set)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("SETBIT"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(offset.ToString()));
            if (set) m.Add(new RESPToken("1"));
            else m.Add(new RESPToken("0"));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE setex(String key, int sec, String value)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("SETEX"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(sec.ToString()));
            m.Add(new RESPToken(value));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE setnx(String key, String value)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("SETNX"));
            m.Add(new RESPToken(key));            
            m.Add(new RESPToken(value));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE setrange(String key, int offset, String value)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("SETRANGE"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(offset.ToString()));
            m.Add(new RESPToken(value));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE strlen(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("STRLEN"));
            m.Add(new RESPToken(key));
            return Process(m);
        }
    }
}
}
