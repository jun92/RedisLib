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
            RESPMaker m = new RESPMaker();
            m.Add(new RESPToken("APPEND"));
            m.Add(new RESPToken(key));
            m.Add(new RESPToken(value));
            return Process(m);
        }
    }
}
}
