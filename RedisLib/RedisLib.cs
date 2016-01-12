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
        
        public RedisAsyncConnManager _conn;
        public String _error_msg;
        /// <param name="ip">Redis서버 주소</param>
        /// <param name="port">Redis서버 포트</param>
        /// <param name="auth">Redis서버 비밀번호</param>
        /// <return>없음</return>        
        public Redis(string ip, int port, string auth = "")
        {
            //_conn = new RedisConnManager(ip, port, auth);
            _conn = new RedisAsyncConnManager(ip, port);

            if (!_conn.Connect())
            {
                _error_msg = _conn._error_msg;
                return;
            }

            //RedisClusterSupport rcs = new RedisClusterSupport(_conn);
            //rcs.ConstructClusterConfigInfo();
        }
        public bool reconn(String ip, int port, string auth = "")
        {
            _conn = new RedisAsyncConnManager(ip, port, auth);
            if( !_conn.Connect())
            {
                _error_msg = _conn._error_msg;
                return false;
            }
            return true;
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
            return new RedisStrings(_conn);
        }
        /// <summary>
        /// 레디스의 Hashes타입에 접근하기 위한 객체를 리턴한다. 
        /// </summary>
        /// <returns>해쉬 담당 객체 </returns>
        public RedisHashes GetHashes()
        {
            return new RedisHashes(_conn);
        }
        public RedisLists GetLists()
        {
            return new RedisLists(_conn);
        }
        public RedisConnection GetConnection()
        {
            return new RedisConnection(_conn);
        }
        public RedisKeys GetKeys()
        {
            return new RedisKeys(_conn);
        }
        public RedisSets GetSets()
        {
            return new RedisSets(_conn);
        }
        public RedisClusterSupport GetClusterSupport()
        {
            return new RedisClusterSupport(_conn);
        }
    }
    
    
    
    
} // end of namepsace Redis 
} // end of namespace Syncnet 
