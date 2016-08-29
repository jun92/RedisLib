using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO; 


namespace Syncnet 
{
namespace RedisLib
{   
    /// <summary>레디스 기본 클래스(메인)</summary>
    public class Redis
    {        
        private bool _IsCluster;
        public RedisAsyncConnManager _connManager;
        RedisClusterSupport _rcs;
        public String _error_msg;
        /// <param name="ip">Redis서버 주소</param>
        /// <param name="port">Redis서버 포트</param>
        /// <param name="auth">Redis서버 비밀번호</param>
        /// <return>없음</return>        
        public Redis(string ip, int port, bool IsCluster = false)
        {        
            _connManager = new RedisAsyncConnManager(ip, port);
            _IsCluster = IsCluster;

            if (!_connManager.Connect())
            {
                _error_msg = _connManager._error_msg;
                return;
            }

            if( IsCluster )
            {
                _rcs = new RedisClusterSupport(_connManager);
                _rcs.ConstructClusterConfigInfo();
                _connManager.SetClusterEnable();
            }
        }        
        
        /// <summary>
        /// 에러 메세지를 리턴 
        /// </summary>
        /// <returns>에러 메세지가 포함되어 있다.</returns>
        public String GetErrorMsg()
        {
            return _error_msg;
        }
        /// <summary>
        /// 레디스의 String 타입에 접근하기 위한 객체를 리턴한다. 
        /// </summary>
        /// <returns>String 담당 객체</returns>
        public RedisStrings GetStrings()
        {
            return new RedisStrings(_connManager);
        }
        /// <summary>
        /// 레디스의 Hashes타입에 접근하기 위한 객체를 리턴한다. 
        /// </summary>
        /// <returns>해쉬 담당 객체 </returns>
        public RedisHashes GetHashes()
        {
            if (_IsCluster) return new RedisHashes(_connManager, _rcs);
            return new RedisHashes(_connManager);
        }
        public RedisLists GetLists()
        {
            return new RedisLists(_connManager);
        }
        public RedisConnection GetConnection()
        {
            return new RedisConnection(_connManager);
        }
        public RedisKeys GetKeys()
        {
            return new RedisKeys(_connManager);
        }
        public RedisSets GetSets()
        {
            return new RedisSets(_connManager);
        }
        public RedisClusterSupport GetClusterSupport()
        {
            return new RedisClusterSupport(_connManager);
        }
        public RedisServer GetServer()
        {
            return new RedisServer(_connManager);
        }
    }
} // end of namepsace Redis 
} // end of namespace Syncnet 
