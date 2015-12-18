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