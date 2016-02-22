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
        protected RedisAsyncConnManager _conn;
        protected RedisClusterSupport _rcs;
        private RedisRESP2Class _rr;

        public RedisObject(RedisAsyncConnManager conn)
        {
            this._conn = conn;
            _rr = new RedisRESP2Class();
        }
        public RedisObject(RedisAsyncConnManager conn, RedisClusterSupport rcs)
        {
            this._conn = conn;
            this._rcs = rcs;
            _rr = new RedisRESP2Class();

        }
        protected void AdjustClusterServer(String key)
        {
            if (_conn.IsClusterEnable() ) _rcs.ReconnAccordingToKey(key);
        }
        protected REDIS_RESPONSE_TYPE Process(RESPMaker m)
        {   
            _conn.Request(m, ref _rr);

            // _rr.response_type이 ERROR이고 메세지가 -MOVED이고 cluster 설정 상태이면 
            // clusterinfo를 재구축하고, 해당 쿼리를 다시 보낸다. 
            if (_rr.response_type == REDIS_RESPONSE_TYPE.ERROR)
            {
                String error_string = getError();
                if (error_string.StartsWith("MOVED"))
                {
                    char[] d = { ' ' };
                    String[] MovedInfo = error_string.Split(d); // 0 - MOVED, 1- hashslot num 2-server:port
                    _rcs.CreateClusterConfigInfo();
                }
            }
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
        public String getError()
        {
            return _rr.getAsString();
        }
    }
}
}