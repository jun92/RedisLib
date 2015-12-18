using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet
{
namespace RedisLib
{
    public class Keys : RedisObject
    {
        public Keys(RedisConnManager conn) : base (conn)
        {
        }
        public REDIS_RESPONSE_TYPE del(params String[] keys)
        {
            RESPMaker m = new RESPMaker();
            RESPToken cmd = new RESPToken("DEL");

            m.Add(cmd);
            foreach(String s in keys)
            {
                RESPToken resp_key = new RESPToken(s);
                m.Add(resp_key);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE dump(String key)
        {
            RESPMaker m = new RESPMaker();
            RESPToken cmd = new RESPToken("DUMP");
            RESPToken resp_key = new RESPToken(key);

            m.Add(cmd);
            m.Add(resp_key);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE exists(params String[] keys)
        {
            RESPMaker m = new RESPMaker(); 
            RESPToken cmd = new RESPToken("EXISTS");

            m.Add(cmd);
            foreach (String s in keys)
            {
                RESPToken resp_key = new RESPToken(s);
                m.Add(resp_key);
            }
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE expire(String key, int seconds)
        {
            RESPMaker m = new RESPMaker(); 
            RESPToken cmd = new RESPToken("EXPIRE");
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_seconds = new RESPToken(seconds.ToString());

            m.Add(cmd);
            m.Add(resp_key);
            m.Add(resp_seconds);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE expireat(String key, int unix_timestamp)
        {
            RESPMaker m = new RESPMaker(); 
            RESPToken cmd = new RESPToken("EXPIREAT");
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_time = new RESPToken(unix_timestamp.ToString());

            m.Add(cmd);
            m.Add(resp_key);
            m.Add(resp_time);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE keys(String pattern)
        {
            RESPMaker m = new RESPMaker(); 
            RESPToken cmd = new RESPToken("KEYS");
            RESPToken resp_pattern = new RESPToken(pattern);
            m.Add(cmd);
            m.Add(resp_pattern);
            return Process(m);
        }

        
        /*
        public REDIS_RESPONSE_TYPE ()
        {
            RESPMaker m = new RESPMaker(); 
            RESPToken cmd = new RESPToken("");

            return Process(m);
        }
        */
    }
}
}