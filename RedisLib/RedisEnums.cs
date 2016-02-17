using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{ 
namespace RedisLib
{
    public enum REDIS_RESPONSE_TYPE
    {
        ERROR = 0,
        INT = 1,
        SSTRING = 2,
        BSTRING = 3,
        ARRAY = 4,
        NOT_ENOUGH_DATA = 5, // 너무 많은 데이타를 받을 경우, 모든 데이타가 도착하지 않았다. 
        NOT_IMPLEMENTED = 6 // not implemented yet.
    } 
}
}