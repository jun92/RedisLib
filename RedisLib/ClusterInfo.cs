using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisNodeConnInfo
    {
        public String server_ip;
        public int server_port;
    }
    
    public class RedisClusterNode
    {
        public int lowHashSlot;
        public int highHashSlot;
        public List<RedisNodeConnInfo> ServerConnInfo;        

        public RedisClusterNode()
        {
            ServerConnInfo = new List<RedisNodeConnInfo>();
        }
    }
    [Serializable]
    public class RedisClusterInfo
    {
        public List<RedisClusterNode> info;

        public RedisClusterInfo()
        {
            info = new List<RedisClusterNode>();
        }
        public void Add(RedisClusterNode node)
        {
            info.Add(node);
        }
    }
}
}
