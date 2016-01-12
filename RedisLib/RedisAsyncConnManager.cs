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
    
    public class RedisAsyncConnManager
    {
        public const int NET_RECV_SIZE = 4096;
        Socket _conn;
        IPEndPoint ipep;        
        public String _error_msg;
        public String _RecvString;
        public RedisRESP2Class rr;

        public RedisAsyncConnManager(string ip, int port, string auth = null)
        {
            ipep    = new IPEndPoint(IPAddress.Parse(ip), port);
            _conn   = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            rr      = new RedisRESP2Class();
        }
        public bool Connect()
        {
            try
            {
                AsyncPassParamConnect param = new AsyncPassParamConnect();
                param.socket = _conn;                 
                _conn.BeginConnect(ipep, new AsyncCallback(OnCompleteConnect), param);
                param.wait();
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
        private void OnCompleteConnect(IAsyncResult IAR)
        {
            try
            {
                AsyncPassParamConnect param = (AsyncPassParamConnect)IAR.AsyncState;
                param.set();
            }
            catch (SocketException e)
            {
                _error_msg = e.ToString();
                return;
            }
        }
        public bool Send(String RESPTokens)
        {
            AsyncPassParamSend param = new AsyncPassParamSend();
            
            param.SetSocket(_conn);
            param.AllocOutputBuffer(RESPTokens.Length);           
            
            RedisSerializer rs = new RedisSerializer();
            
            rs.Serialize(RESPTokens, ref param.data_out, ref param.SendSize);

            try            
            {
                _conn.BeginSend(
                    param.data_out, 
                    0, 
                    param.SendSize, 
                    SocketFlags.None, 
                    new AsyncCallback(OnCompletedSend), 
                    param);
                param.wait();
            }
            catch( SocketException e )
            {
                String em = e.Message;
                return false;
            }
            return true;
        }
        private void OnCompletedSend(IAsyncResult IAR)
        {
            try
            {
                AsyncPassParamSend param = (AsyncPassParamSend)IAR.AsyncState;
                int bytesSent = param.GetSocket().EndSend(IAR);

                if (bytesSent < param.SendSize)
                {                    
                    param.GetSocket().BeginSend(
                        param.data_out, 
                        bytesSent, 
                        param.SendSize - bytesSent, 
                        SocketFlags.None, 
                        new AsyncCallback(OnCompletedSend), 
                        param);
                }
                else
                    param.set();
            }
            catch (SocketException e)
            {
                String em = e.Message;
            }
        }

        public bool Recv()
        {
            AsyncPassParamRecv param = new AsyncPassParamRecv();

            param.AllocBuffer(NET_RECV_SIZE);
            param.SetSocket(_conn);

            try
            {
                _conn.BeginReceive(
                    param.data_in, 
                    0, 
                    NET_RECV_SIZE, 
                    SocketFlags.None, 
                    new AsyncCallback(OnCompletedReceive), 
                    param);
                param.wait();

                RedisSerializer rs = new RedisSerializer();
                rs.Deserialize(param.rb.GetBuffer(), param.rb.GetSize(), ref _RecvString);
                
            }
            catch(SocketException e )
            {
                String em = e.Message;

            }
            return true;
        }
        
        private void OnCompletedReceive(IAsyncResult IAR)
        {
            NetRecvBuffer rb                = new NetRecvBuffer();
            RedisSerializer rs              = new RedisSerializer();
            AsyncPassParamRecv param        = new AsyncPassParamRecv();
            RedisRESP2Class rr_for_check    = new RedisRESP2Class();
            try 
            {   
                param = (AsyncPassParamRecv)IAR.AsyncState;
                int bytesRead = param.GetSocket().EndReceive(IAR);

                // 버퍼에 추가하고 
                param.rb.Add(param.data_in, bytesRead);
                if (rr_for_check.IsCompletedPacket(param.rb.GetString())) param.set();
                else
                    param.GetSocket().BeginReceive(
                        param.data_in, 
                        0, 
                        NET_RECV_SIZE, 
                        SocketFlags.None, 
                        new AsyncCallback(OnCompletedReceive), 
                        param);
            }
            catch(SocketException e)
            {
                String em = e.Message;
            }
        }
        
        public String Request(RESPMaker req)
        {
            Send(req.Make().ToString());
            Recv();
            return _RecvString;
        }

    }
}
}