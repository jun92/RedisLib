using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisClusterNode
    {
        public int lowHashSlot;
        public int highHashSlot;
        public String MasterServer;
        public int MasterPort; 
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
