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
        public RedisKeys(RedisAsyncConnManager conn)
            : base(conn)
        {            
        }
        public REDIS_RESPONSE_TYPE del(params String[] keys)
        {
            RESPMaker m = new RESPMaker();            
            m.Add("DEL");
            foreach(String s in keys)
            {                
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE dump(String key)
        {
            RESPMaker m = new RESPMaker();         

            m.Add("DUMP");
            m.Add(key);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE exists(params String[] keys)
        {
            RESPMaker m = new RESPMaker();             

            m.Add("EXISTS");
            foreach (String s in keys)
            {                
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE expire(String key, int seconds)
        {
            RESPMaker m = new RESPMaker();            

            m.Add("EXPIRE");
            m.Add(key);
            m.Add(seconds.ToString());

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE expireat(String key, int unix_timestamp)
        {
            RESPMaker m = new RESPMaker(); 
            m.Add("EXPIREAT");
            m.Add(key);
            m.Add(unix_timestamp.ToString());
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE keys(String pattern)
        {
            RESPMaker m = new RESPMaker(); 
            m.Add("KEYS");
            m.Add(pattern);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE migrate(String host, int port, String key, String dest_db, bool IsCopy)
        {
            RESPMaker m = new RESPMaker();
            m.Add("MIGRATE");
            m.Add(host);
            m.Add(port.ToString());
            m.Add(key);
            m.Add(dest_db);
            if (IsCopy)     m.Add("COPY");
            else            m.Add("REPLACE");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE move(String key, String db)
        {
            RESPMaker m = new RESPMaker();            

            m.Add("MOVE");
            m.Add(key);
            m.Add(db);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE Object(String subcommand, params String[] arguments)
        {
            RESPMaker m = new RESPMaker();            

            m.Add("OBJECT");
            m.Add(subcommand);

            foreach( String argu in arguments)
            {                
                m.Add(argu);
            }
            return Process(m);            
        }
        public REDIS_RESPONSE_TYPE persist(String key)
        {
            RESPMaker m = new RESPMaker();
            
            m.Add("PERSIST");
            m.Add(key);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE pexpire(String key, long milliseconds)
        {
            RESPMaker m = new RESPMaker();

            m.Add("PEXPIRE");
            m.Add(key);
            m.Add(milliseconds.ToString());

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE pexpireat(String key, String unixtimestamp_milliseconds)
        {
            RESPMaker m = new RESPMaker();
            
            m.Add("PEXPIREAT");
            m.Add(key);
            m.Add(unixtimestamp_milliseconds);

            return Process(m);
        }

        public REDIS_RESPONSE_TYPE pttl(String key)
        {
            RESPMaker m = new RESPMaker();
            
            m.Add("PTTL");
            m.Add(key);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE randomkey()
        {
            RESPMaker m = new RESPMaker();
            m.Add("RANDOMKEY");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE rename(String key, String new_key)
        {
            RESPMaker m = new RESPMaker();

            m.Add("RENAME");
            m.Add(key);
            m.Add(new_key);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE renamenx(String key, String new_key)
        {
            RESPMaker m = new RESPMaker();

            m.Add("RENAMENX");
            m.Add(key);
            m.Add(new_key);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE restore(String key, String ttl, String serialized_val, bool IsReplace = false )
        {
            RESPMaker m = new RESPMaker();

            m.Add("RESTORE");
            m.Add(key);
            m.Add(ttl);
            m.Add(serialized_val); 

            if( IsReplace) m.Add("REPLACE");

            return Process(m);
        }
        //public REDIS_RESPONSE_TYPE sort(String key, String Pattern = null, String limit_offset, String limit_count,  )
        public REDIS_RESPONSE_TYPE scan(String cursor, String pattern = null , String count = null )
        {
            RESPMaker m = new RESPMaker();            

            m.Add("SCAN");
            m.Add(cursor);

            if(!String.IsNullOrEmpty( pattern ))    m.Add(pattern);            
            if(!String.IsNullOrEmpty( count))       m.Add(count);            
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE ttl(String key) 
        {
            RESPMaker m = new RESPMaker();
            m.Add("TTL");
            m.Add(key);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE type(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add("TYPE");
            m.Add(key);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE wait(int numslaves, int timeout)
        {
            RESPMaker m = new RESPMaker();
            m.Add("WAIT");
            m.Add(numslaves.ToString());
            m.Add(timeout.ToString());
            return Process(m);
        }
    }
}
}