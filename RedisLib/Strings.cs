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
        
        public RedisStrings(RedisConnManager conn) : base(conn)
        {            
        }
        public REDIS_RESPONSE_TYPE Append(string key, string value)
        {
            RESPToken resp_key = new RESPToken(key);
            RESPToken resp_value = new RESPToken(value);
            RESPToken Command = new RESPToken("APPEND");
            RESPMaker m = new RESPMaker();

            m.Add(Command);
            m.Add(resp_key);
            m.Add(resp_value);

            return Process(m);
        }
    }
}
}
