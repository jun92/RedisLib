using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisZSets : RedisObject
    {
        public enum ZSET_AGGREGATE_TYPE
        {
            NONE = 0,
            SUM = 1,
            MIN = 2,
            MAX = 3
        }
        public enum ZSET_ADD_TYPE
        {
            NONE = 0,
            NX = 1,
            XX = 2,
            CH = 4,
            INCR = 8
        }

        public RedisZSets(RedisAsyncConnManager conn)
            : base(conn)
        {
        }
        public REDIS_RESPONSE_TYPE zadd(String key, ZSET_ADD_TYPE options, String score, String member, params String[] scorenmembers)
        {
            if( (scorenmembers.Length %2) != 0 ) return REDIS_RESPONSE_TYPE.ERROR;

            RESPMaker m = new RESPMaker();
            m.Add("ZADD");
            m.Add(key);

            if((options & ZSET_ADD_TYPE.NX) != 0 && (options & ZSET_ADD_TYPE.XX) == 0) m.Add("NX");
            if((options & ZSET_ADD_TYPE.XX) != 0 && (options & ZSET_ADD_TYPE.NX) == 0) m.Add("XX");

            if ((options & ZSET_ADD_TYPE.CH) != 0) m.Add("CH");
            if ((options & ZSET_ADD_TYPE.INCR) != 0) m.Add("INCR");

            m.Add(score);
            m.Add(member);
            foreach ( String s in scorenmembers)
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zadd(String key, ZSET_ADD_TYPE options, Dictionary<String, String> scorenmembers)
        {
            if (scorenmembers.Count == 0) return REDIS_RESPONSE_TYPE.ERROR;
            RESPMaker m = new RESPMaker();
            m.Add("ZADD");
            m.Add(key);

            if ((options & ZSET_ADD_TYPE.NX) != 0 && (options & ZSET_ADD_TYPE.XX) == 0) m.Add("NX");
            if ((options & ZSET_ADD_TYPE.XX) != 0 && (options & ZSET_ADD_TYPE.NX) == 0) m.Add("XX");

            if ((options & ZSET_ADD_TYPE.CH) != 0) m.Add("CH");
            if ((options & ZSET_ADD_TYPE.INCR) != 0) m.Add("INCR");
            
            foreach (KeyValuePair<String, String> s in scorenmembers)
            {
                m.Add(s.Key);
                m.Add(s.Value);
            }
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE zcard(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZCARD");
            m.Add(key);
            return Process(m);
        }
        /// <summary>
        /// ZCOUNT명령어를 실행시킨다. min,max에 0을 지정하면 -inf, +inf로 요청한다. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        /// <seealso cref="http://redis.io/commands/zcount"/>
        public REDIS_RESPONSE_TYPE zcount(String key, int min, int max)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZCOUNT");
            m.Add(key);
            if (min == 0) m.Add("-inf");
            else m.Add(min.ToString());
            if (max == 0) m.Add("+inf");
            else m.Add(max.ToString());
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zincrby(String key, int increment, String member)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZINCRBY");
            m.Add(key);
            m.Add(increment.ToString());
            m.Add(member);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zinterstore(String dest, List<String> keys, List<int> weights, ZSET_AGGREGATE_TYPE Aggregate = ZSET_AGGREGATE_TYPE.NONE)
        {
            if (keys.Count == 0) return REDIS_RESPONSE_TYPE.ERROR;
            /*
            if (keys.Count != weights.Count) return REDIS_RESPONSE_TYPE.ERROR;
            if (keys.Count == 0) return REDIS_RESPONSE_TYPE.ERROR;
            if (weights.Count == 0) return REDIS_RESPONSE_TYPE.ERROR;
            */

            RESPMaker m = new RESPMaker();

            m.Add("ZINTERSCORE");
            m.Add(dest);
            m.Add(keys.Count);
            foreach( String key in keys)
            {
                m.Add(key);
            }
            m.Add("WEIGHTS");
            foreach( int weight in weights)
            {
                m.Add(weight);
            }
            if (Aggregate != ZSET_AGGREGATE_TYPE.NONE)
            {
                m.Add("AGGREGATE");
                switch (Aggregate)
                {
                    case ZSET_AGGREGATE_TYPE.SUM: m.Add("SUM"); break;
                    case ZSET_AGGREGATE_TYPE.MAX: m.Add("MAX"); break;
                    case ZSET_AGGREGATE_TYPE.MIN: m.Add("MIN"); break;
                }
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zlexcount(String key, String min, String max)
        {
            RESPMaker m = new RESPMaker();

            m.Add("ZLEXCOUNT");
            m.Add(key);
            m.Add(min);
            m.Add(max);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zrange(String key, int start, int stop, bool IsWithScore = false)
        {
            RESPMaker m = new RESPMaker();

            m.Add("ZRANGE");
            m.Add(key);
            m.Add(start.ToString());
            m.Add(stop.ToString());
            if (IsWithScore) m.Add("WITHSCORES");

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zrangebylex(String key, String min, String max, int offset = 0, int count = 0 )
        {
            if ((offset != 0 && count == 0) &&
                (offset == 0 && count != 0)) return REDIS_RESPONSE_TYPE.ERROR;
            
            RESPMaker m = new RESPMaker();
            m.Add("ZRANGEBYLEX");
            m.Add(key);
            m.Add(min);
            m.Add(max);
            if( offset != 0 && count != 0 )
            {
                m.Add("LIMIT");
                m.Add(offset.ToString());
                m.Add(count.ToString());
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zrangebyscore(String key, String min, String max, bool IsWithScore = false, int offset = 0, int count = 0)
        {
            if ((offset != 0 && count == 0) &&
                (offset == 0 && count != 0)) return REDIS_RESPONSE_TYPE.ERROR;

            RESPMaker m = new RESPMaker();
            m.Add("ZRANGEBYSCORE");
            m.Add(key);
            m.Add(min);
            m.Add(max);
            
            if (IsWithScore) m.Add("WITHSCORES");

            if (offset != 0 && count != 0)
            {
                m.Add("LIMIT");
                m.Add(offset.ToString());
                m.Add(count.ToString());
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zrank(String key, String member)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZRANK");
            m.Add(key);
            m.Add(member);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zrem(String key, String member, params String[] members)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZREM");
            m.Add(key);
            m.Add(member);
            foreach( String s in members)
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zremrangebylex(String key, String min, String max)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZREMRANGEBYLEX");
            m.Add(key);
            m.Add(min);
            m.Add(max);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zremrangebyrank(String key, int start, int stop)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZREMRANGEBYRANK");
            m.Add(key);
            m.Add(start.ToString());
            m.Add(stop.ToString());
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zremrangebyscore(String key, String min, String max)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZREMRANGEBYSCORE");
            m.Add(key);
            m.Add(min);
            m.Add(max);
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE zrevrange(String key, int start, int stop, bool IsWithScores = false)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZREVRANGE");
            m.Add(key);
            m.Add(start.ToString());
            m.Add(stop.ToString());
            if (IsWithScores) m.Add("WITHSCORES");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zrevrangebylex(String key, String max, String min, int offset=0, int count = 0)
        {
            if ((offset != 0 && count == 0) &&
                (offset == 0 && count != 0)) return REDIS_RESPONSE_TYPE.ERROR;

            RESPMaker m = new RESPMaker();
            m.Add("ZREVRANGEBYLEX");
            m.Add(key);
            m.Add(max);
            m.Add(min);
            if (offset != 0 && count != 0)
            {
                m.Add("LIMIT");
                m.Add(offset.ToString());
                m.Add(count.ToString());
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zrevrangebyscore(String key, String max, String min, bool IsWithScores = false, int offset = 0, int count = 0)
        {
            if ((offset != 0 && count == 0) &&
                (offset == 0 && count != 0)) return REDIS_RESPONSE_TYPE.ERROR;

            RESPMaker m = new RESPMaker();
            m.Add("ZREVRANGEBYSCORE");
            m.Add(key);
            m.Add(max);
            m.Add(min);

            if (IsWithScores) m.Add("WITHSCORES");

            if (offset != 0 && count != 0)
            {
                m.Add("LIMIT");
                m.Add(offset.ToString());
                m.Add(count.ToString());
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zrevrank(String key, String member)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZREVRANK");
            m.Add(key);
            m.Add(member);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zscan(String key, String cursor, String pattern = null, String count = null)
        {
            RESPMaker m = new RESPMaker();

            m.Add("ZSCAN");
            m.Add(key);
            m.Add(cursor);

            if (!String.IsNullOrEmpty(pattern)) m.Add(pattern);
            if (!String.IsNullOrEmpty(count)) m.Add(count);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE zscore(String key, String member)
        {
            RESPMaker m = new RESPMaker();
            m.Add("ZSCORE");
            m.Add(key);
            m.Add(member);
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE zunionstore(String dest, List<String> keys, List<int> weights, ZSET_AGGREGATE_TYPE aggregate = ZSET_AGGREGATE_TYPE.NONE)
        {
            if (keys.Count == 0) return REDIS_RESPONSE_TYPE.ERROR;

            RESPMaker m = new RESPMaker();
            m.Add("ZUNIONSTORE");
            m.Add(dest);

            m.Add(keys.Count);

            foreach( String s in keys)
            {
                m.Add(s);
            }
            if (weights.Count > 0) m.Add("WEIGHTS");

            foreach( int weight in weights)
            {
                m.Add(weight.ToString());
            }
            return Process(m);
        }


    }
}
}
