using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisClusterSupport : RedisObject
    {
        public RedisClusterSupport(RedisConnManager conn) : base(conn)
        {

        }
        public REDIS_RESPONSE_TYPE clusterslots()
        {
            RESPMaker m = new RESPMaker();

            m.Add("CLUSTER");
            m.Add("SLOTS");

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE clusternodes()
        {
            RESPMaker m = new RESPMaker();
            m.Add("CLUSTER");
            m.Add("NODES");
            return Process(m);
        }

        public void ConstructClusterConfigInfo()
        {
            List<dynamic> narray = new List<dynamic>();
            clusterslots();
            getNestedArray(ref narray);


            
        }
    }
}
}
