using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{
namespace RedisLib
{
    public class RedisServer : RedisObject
    {
        public RedisServer(RedisConnManager conn) : base(conn)
        {
        }
        public REDIS_RESPONSE_TYPE bgrewriteaof()
        {
            RESPMaker m = new RESPMaker();
            m.Add("BGREWRITEAOF");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE bgsave()
        {
            RESPMaker m = new RESPMaker();
            m.Add("BGSAVE");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE clientgetname()
        {
            RESPMaker m = new RESPMaker();
            m.Add("CLIENT");
            m.Add("GETNAME");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE clientkill()
        {
            return REDIS_RESPONSE_TYPE.NOT_IMPLEMENTED;
        }
        public REDIS_RESPONSE_TYPE clientlist()
        {
            RESPMaker m = new RESPMaker();
            m.Add("CLIENT");
            m.Add("LIST");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE clientpause(int timeout)
        {
            RESPMaker m = new RESPMaker();
            m.Add("CLIENT");
            m.Add("PAUSE");
            m.Add(timeout);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE clientsetname(String conn_name)
        {
            RESPMaker m = new RESPMaker();
            m.Add("CLIENT");
            m.Add("SETNAME");
            m.Add(conn_name);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE command()
        {
            RESPMaker m = new RESPMaker();
            m.Add("COMMAND");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE commandcount()
        {
            RESPMaker m = new RESPMaker();
            m.Add("COMMAND");
            m.Add("COUNT");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE commandgetkeys()
        {
            return REDIS_RESPONSE_TYPE.NOT_IMPLEMENTED;
        }
        public REDIS_RESPONSE_TYPE commandinfo(String command_name, params String[] command_names)
        {
            RESPMaker m = new RESPMaker();

            m.Add("COMMAND");
            m.Add("INFO");
            m.Add(command_name);

            foreach(String s in command_names)
            {
                m.Add(s);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE configget(String parameter)
        {
            RESPMaker m = new RESPMaker();

            m.Add("CONFIG");
            m.Add("GET");
            m.Add(parameter);

            return Process(m);        
        }
        public REDIS_RESPONSE_TYPE configresetstat()
        {
            RESPMaker m = new RESPMaker();

            m.Add("CONFIG");
            m.Add("RESETSTAT");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE configrewrite()
        {
            RESPMaker m = new RESPMaker();

            m.Add("CONFIG");
            m.Add("REWRITE");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE configset(String parameter, String value)
        {
            RESPMaker m = new RESPMaker();

            m.Add("CONFIG");
            m.Add("SET");
            m.Add(parameter);
            m.Add(value);
            return Process(m);
        }

        public REDIS_RESPONSE_TYPE dbsize()
        {
            RESPMaker m = new RESPMaker();
            m.Add("DBSIZE");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE debugobject(String key)
        {
            RESPMaker m = new RESPMaker();
            m.Add("DEBUG");
            m.Add("OBJECT");
            m.Add(key);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE debugsegfault()
        {
            return REDIS_RESPONSE_TYPE.NOT_IMPLEMENTED;
        }
        public REDIS_RESPONSE_TYPE flushall()
        {
            RESPMaker m = new RESPMaker();
            m.Add("FLUSHALL");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE flushdb()
        {
            RESPMaker m = new RESPMaker();
            m.Add("FLUSHDB");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE info(String section)
        {
            RESPMaker m = new RESPMaker();
            m.Add("INFO");
            m.Add(section);
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE lastsave()
        {
            RESPMaker m = new RESPMaker();
            m.Add("LASTSAVE");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE monitor()
        {
            return REDIS_RESPONSE_TYPE.NOT_IMPLEMENTED;
        }
        public REDIS_RESPONSE_TYPE role()
        {
            RESPMaker m = new RESPMaker();
            m.Add("ROLE");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE save()
        {
            RESPMaker m = new RESPMaker();
            m.Add("SAVE");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE shutdown(bool IsSave = false)
        {
            RESPMaker m = new RESPMaker();

            m.Add("SHUTDOWN");
            if (IsSave) m.Add("SAVE");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE saveof(String host, int port)
        {
            RESPMaker m = new RESPMaker();

            m.Add("SLAVEOF");
            m.Add(host);
            m.Add(port);

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE slowlog(String subcommand, params String[] arguments)
        {
            RESPMaker m = new RESPMaker();

            m.Add("SLOWLOG");
            m.Add(subcommand);

            foreach(String argument in arguments)
            {
                m.Add(argument);
            }
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE sync()
        {
            RESPMaker m = new RESPMaker();
            m.Add("SYNC");
            return Process(m);
        }
        public REDIS_RESPONSE_TYPE time()
        {
            RESPMaker m = new RESPMaker();
            m.Add("TIME");
            return Process(m);
        }
    }

}
}
