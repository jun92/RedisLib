using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO; 

namespace Syncnet
{ 
namespace RedisLib
{
    /// <summary>
    /// 레디스 서버와 소켓통신을 담당하는 클래스 
    /// </summary>
    public class RedisConnManager
    {
        /// <summary>
        /// 기본 통신 버퍼 크기 지정 
        /// </summary>
        public const int NET_RECV_SIZE = 4096;
        Socket _conn;
        IPEndPoint ipep;
        public String _error_msg;
        public String _RecvString;
        public RedisRESP2Class rr;
        public RedisConnManager(string ip, int port, string auth)
        {
            ipep = new IPEndPoint(IPAddress.Parse(ip), port);
            _conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            rr = new RedisRESP2Class();

            if (auth.Length != 0)
            {
                //implement later. 
            }
        }
        public bool Connect()
        {
            try
            {
                _conn.Connect(ipep);
            }
            catch (SocketException e)
            {
                _error_msg = e.ToString();
                return false;
            }
            return true;
        }
        public bool Close()
        {
            _conn.Close();
            return true;
        }
        public bool Send(String RESPTokens)
        {
            byte[] data_out = new Byte[RESPTokens.Length];
            int size = 0;
            RedisSerializer rs = new RedisSerializer();

            rs.Serialize(RESPTokens, ref data_out, ref size);

            // 블럭킹 콜을 사용하기때문에 보낸 데이타 추적을 할 필요가 없다. 
            try
            {
                _conn.Send(data_out);
            }
            catch (SocketException e)
            {
                String em = e.Message;

            }
            return true;
        }
        public bool Recv()
        {
            byte[] recv_buffer = new byte[NET_RECV_SIZE];
            int recvBytes;
            //bool bFirst = true;

            NetRecvBuffer rb = new NetRecvBuffer();
            RedisSerializer rs = new RedisSerializer();

            do
            {

                recvBytes = _conn.Receive(recv_buffer);
                rb.Add(recv_buffer, recvBytes);
                //Console.WriteLine("Available:{0}", _conn.Available);

                
                //if (bFirst) bFirst = false;
                //else System.Threading.Thread.Sleep(1);

            } while (_conn.Available != 0);

            rs.Deserialize(rb.GetBuffer(), rb.GetSize(), ref _RecvString);
            return true;
        }
        public String Request(RESPMaker req)
        {
            // Reqest & response
            Send(req.Make().ToString());
            Recv();
            return _RecvString;
        }


    }
    
}
}