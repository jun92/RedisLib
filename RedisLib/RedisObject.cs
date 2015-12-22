using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisObject
    {
        private RedisConnManager _conn;
        private RedisRESP2Class _rr;

        public RedisObject(RedisConnManager conn)
        {
            this._conn = conn;
            _rr = new RedisRESP2Class();
        }
        protected REDIS_RESPONSE_TYPE Process(RESPMaker m)
        {
            _rr.parse(_conn.Request(m));
            return _rr.response_type;
        }

        public String getString()
        {
            return _rr.getAsString();
        }
        public int getInt()
        {
            return _rr.getAsInt();
        }
        public bool getHashes(ref Dictionary<String, String> dic)
        {
            _rr.getAsDictionary(ref dic);
            return true;
        }
        public bool getDictionary(ref Dictionary<String, String> dic)
        {
            _rr.getAsDictionary(ref dic);
            return true;
        }
        public bool getLists(ref List<String> list)
        {
            _rr.getAsLists(ref list);
            return true;
        }        
        public bool getNestedArray(ref List<dynamic> narray)
        {
            _rr.getAsNestedArray(ref narray);
            return true;
        }



    }
}
}