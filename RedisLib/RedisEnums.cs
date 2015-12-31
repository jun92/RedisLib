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
        NOT_IMPLEMENTED = 5 // not implemented yet.
    } 
}
}