using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet
{
    namespace RedisLib
    {
        public class RESPNode
        {
            public String Value;
            public REDIS_RESPONSE_TYPE Type;

            public int TotalCount;
            public int CurrentCount;

            public RESPNode parent;
            public RESPNode next;
            public RESPNode prev;
            public RESPNode child;

            public RESPNode()
            {
                parent = null;
                next = null;
                prev = null;
                child = null;
            }
            
            public void Set(String respString)
            {
            }            
        }
    }
}