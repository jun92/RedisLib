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
    public class AsyncObject
    {
        protected AutoResetEvent done;
        public AsyncObject()
        {
            done = new AutoResetEvent(false);
        }
        public void wait()
        {
            done.WaitOne();
        }
        public void set()
        {
            done.Set();
        }
    }

    public class AsyncPassParamConnect : AsyncObject
    {
        public Socket socket;
        public AsyncPassParamConnect()
            : base()
        {
        }
    }
    public class AsyncPassParamSend : AsyncObject
    {
        private Socket socket;
        public int SendSize;        
        public byte[] data_out;
        

        public AsyncPassParamSend()
            : base()
        {
        }        
        public void AllocOutputBuffer(int size)
        {
            data_out = new Byte[size];
        }
        public void SetSocket(Socket s)
        {
            socket = s;
        }
        public Socket GetSocket()
        {
            return socket;
        }
    }
    public class AsyncPassParamRecv : AsyncObject
    {
        private Socket socket;
        public byte[] data_in;
        public int recv_size;
        public NetRecvBuffer rb;
        public RedisRESP2Class _rr; 

        public AsyncPassParamRecv()  : base()
        {
            rb = new NetRecvBuffer();
        }
        public void AllocBuffer(int size)
        {
            data_in = new Byte[size];
        }
        public void SetSocket(Socket s)
        {
            socket = s;
        }
        public Socket GetSocket()
        {
            return socket;
        }
        public void SetRedisRESP(ref RedisRESP2Class rr)
        {
            _rr = rr;
        }
    }
}
}
