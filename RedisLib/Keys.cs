using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet
{
namespace RedisLib
{
    public class RedisKeys : RedisObject
    {
        public RedisKeys(RedisConnManager conn)
            : base(conn)
        {            
        }
        public REDIS_RESPONSE_TYPE del(params String[] keys)
        {
            RESPMaker m = new RESPMaker();            
            m.Add(new RESPToken("DEL"));
            foreach(String s in keys)
            {                
                m.Add(new RESPToken(s));
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE dump(String key)
        {
            RESPMaker m = new RESPMaker();         

            m.Add(new RESPToken("DUMP"));
            m.Add(new RESPToken(key));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE exists(params String[] keys)
        {
            RESPMaker m = new RESPMaker();             

            m.Add(new RESPToken("EXISTS"));
            foreach (String s in keys)
            {                
                m.Add(new RESPToken(s));
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE expire(String key, int seconds)
        {
            RESPMaker m = new RESPMaker();            

            m.Add(new RESPToken("EXPIRE"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(seconds.ToString()));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE expireat(String key, int unix_timestamp)
        {
            RESPMaker m = new RESPMaker(); 
            m.Add(new RESPToken("EXPIREAT"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(unix_timestamp.ToString()));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE keys(String pattern)
        {
            RESPMaker m = new RESPMaker(); 
            m.Add(new RESPToken("KEYS"));
            m.Add(new RESPToken(pattern));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE migrate(String host, int port, String key, String dest_db, bool IsCopy)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("MIGRATE"));
            m.Add(new RESPToken(host));
            m.Add(new RESPToken(port.ToString()));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(dest_db));
            if (IsCopy)     m.Add(new RESPToken("COPY"));
            else            m.Add(new RESPToken("REPLACE"));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE move(String key, String db)
        {
            RESPMaker m = new RESPMaker();            

            m.Add(new RESPToken("MOVE"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(db));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE Object(String subcommand, params String[] arguments)
        {
            RESPMaker m = new RESPMaker();            

            m.Add(new RESPToken("OBJECT"));
            m.Add(new RESPToken(subcommand));

            foreach( String argu in arguments)
            {                
                m.Add(new RESPToken(argu));
            }
            return Process(m);            
        }
        public REDIS_RESPONSE_TYPE persist(String key)
        {
            RESPMaker m = new RESPMaker();
            
            m.Add(new RESPToken("PERSIST"));
            m.Add(new RESPToken(key));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE pexpire(String key, long milliseconds)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("PEXPIRE"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(milliseconds.ToString()));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE pexpireat(String key, String unixtimestamp_milliseconds)
        {
            RESPMaker m = new RESPMaker();
            
            m.Add(new RESPToken("PEXPIREAT"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(unixtimestamp_milliseconds.ToString()));

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE pttl(String key)
        {
            RESPMaker m = new RESPMaker();
            
            m.Add(new RESPToken("PTTL"));
            m.Add(new RESPToken(key));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE randomkey()
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("RANDOMKEY"));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rename(String key, String new_key)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("RENAME"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(new_key));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE renamenx(String key, String new_key)
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("RENAMENX"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(new_key));

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE restore(String key, String ttl, String serialized_val, bool IsReplace = false )
        {
            RESPMaker m = new RESPMaker();

            m.Add(new RESPToken("RESTORE"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(ttl));
            m.Add(new RESPToken(serialized_val)); 

            if( IsReplace) m.Add(new RESPToken("REPLACE"));

            return Process(m);
        }
        //public REDIS_RESPONSE_TYPE sort(String key, String Pattern = null, String limit_offset, String limit_count,  )
        public REDIS_RESPONSE_TYPE scan(String cursor, String pattern = null , String count = null )
        {
            RESPMaker m = new RESPMaker();            

            m.Add(new RESPToken("SCAN"));
            m.Add(new RESPToken(cursor));

            if(!String.IsNullOrEmpty( pattern ))    m.Add(new RESPToken(pattern));            
            if(!String.IsNullOrEmpty( count))       m.Add(new RESPToken(count));            
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE ttl(String key) 
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("TTL"));
            m.Add(new RESPToken(key));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE type(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("TYPE"));
            m.Add(new RESPToken(key));
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE wait(int numslaves, int timeout)
        {
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("WAIT"));
            m.Add(new RESPToken(numslaves.ToString()));
            m.Add(new RESPToken(timeout.ToString()));
            return Process(m);
        }
    }
}
}