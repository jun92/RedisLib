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
        public RedisConnManager _conn;
        public String _error_msg;
        /// <param name="ip">Redis서버 주소</param>
        /// <param name="port">Redis서버 포트</param>
        /// <param name="auth">Redis서버 비밀번호</param>
        /// <return>없음</return>        
        public Redis(string ip, int port, string auth = "")
        {
            _conn = new RedisConnManager(ip, port, auth);

            if (!_conn.Connect())
            {
                _error_msg = _conn._error_msg;
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
    }
    public enum REDIS_RESPONSE_TYPE
    {
        ERROR       = 0,
        INT         = 1,
        SSTRING     = 2,
        BSTRING     = 3,
        ARRAY       = 4
    }
    /*public class RedisRESP2Class
    {
        public string               _error_msg;
        public int                  result_int;
        public string               result_string;
        public List<String>         result_array;
        public REDIS_RESPONSE_TYPE  response_type;
        public RedisRESP2Class()
        {            
            result_array = new List<String>();
        }
        
        public void getAsDictionary(ref Dictionary<String, String> dic)
        {
            int i;
            if (result_array.Count == 0) return;
            // 짝수이어야만 한다. 
            if ((result_array.Count % 2) != 0) return;
            for(i = 0; i< result_array.Count; i += 2)
            {
                dic.Add(result_array[i].ToString(), result_array[i + 1].ToString());
            }
        }
        public void getAsLists(ref List<String> list)
        {
            int i;
            if (result_array.Count == 0) return;
            for( i = 0; i < result_array.Count; i++)
            {
                list.Add(result_array[i].ToString());
            }
        }
        public void parse(String RESP)
        {
            if (String.IsNullOrEmpty(RESP)) return;

            if (RESP.StartsWith(":")) // int 
            {
                String[] token = RESP.Split(':', '\r', '\n');
                result_int = Int32.Parse(token[1]);
                response_type = REDIS_RESPONSE_TYPE.INT;
            }
            if (RESP.StartsWith("+")) // simple string 
            {
                String[] token = RESP.Split('+', '\r', '\n');
                result_string = token[1];
                response_type = REDIS_RESPONSE_TYPE.SSTRING;
            }
            if (RESP.StartsWith("$")) // bulk string 
            {
                String[] token = RESP.Split('$', '\r', '\n');                
                result_string = token[3];
                response_type = REDIS_RESPONSE_TYPE.BSTRING;
            }
            if (RESP.StartsWith("-")) // error 
            {
                String[] token = RESP.Split('-', '\r', '\n');
                _error_msg = token[1];
                response_type = REDIS_RESPONSE_TYPE.ERROR;
            }
            if (RESP.StartsWith("*")) // array 
            {                
                int i;
                String[] d = new String[]{ "\r\n"};
                String[] token = RESP.Split(d, StringSplitOptions.RemoveEmptyEntries);
                for (i = 2; i < token.Length; i += 2)
                {
                    result_array.Add(token[i]);
                }
                response_type = REDIS_RESPONSE_TYPE.ARRAY;
            }
        }
    }*/
    /// <summary>
    /// 스트링으로 정리된 RESP와 바이트 배열간의 변환를 담당한다.
    /// </summary>
    public class RedisSerializer
    {
        /// <summary>
        /// 스트링을 바이트 배열로 변환한다. 
        /// </summary>
        /// <param name="data_in">스트링 데이타 </param>
        /// <param name="data_out">바이트 배열</param>
        /// <param name="size">바이트 배열의 크기</param>
        public void Serialize(String data_in, ref byte[] data_out, ref int size)
        {
            data_out = Encoding.ASCII.GetBytes(data_in);
            size = data_out.Length;
        }

        /// <summary>
        /// 바이트 배열을 스트링으로 변환한다. 
        /// </summary>
        /// <param name="data_in">바이트배열</param>
        /// <param name="size">바이트 배열의 크기 </param>
        /// <param name="data_out">변환된 스트링</param>
        public void Deserialize(byte[] data_in, int size, ref String data_out)
        {
            Array.Resize<byte>(ref data_in, size);
            data_out = Encoding.Default.GetString(data_in);
        }
    }
    /// <summary>
    /// 네트워크로부터 받은 데이타를 저장하기 위한 가변 byte배열 관리 클래스 
    /// </summary>
    class NetRecvBuffer
    {
        /// <summary>
        /// 내용을 저장할 버퍼 
        /// </summary>
        private byte[] _buffer;
        /// <summary>
        /// 버퍼의 인덱스, 현재 위치 
        /// </summary>
        private int Index;
        private const int BUFFER_SIZE = 1024;
        /// <summary>
        /// 기본 초기화, 기본 버퍼 크기는 1024로 지정된다. 
        /// </summary>
        public NetRecvBuffer()
        {
            _buffer = new Byte[1024];
            Index = 0;
        }
        /// <summary>
        /// 버퍼에 바이트 배열을 추가하여 저장,
        /// 버퍼가 모자를 경우에는 BUFFER_SIZE만큼 증가 시킨후 추가 
        /// </summary>
        /// <param name="add_buffer">추가시킬 버퍼</param>
        /// <param name="size">버퍼의 크기</param>
        /// <returns>최종 버퍼 크기</returns>
        public int Add(byte[] add_buffer, int size)
        {
            if ((Index + size) > BUFFER_SIZE)
            {
                // 버퍼 크기가 모자르면 재할당을 한다. (x2)
                Array.Resize<byte>(ref _buffer, _buffer.Length + BUFFER_SIZE);
            }
            Array.Copy(add_buffer, 0, _buffer, Index, size);
            Index += size;
            return Index;
        }
        public byte[] GetBuffer()
        {
            return _buffer;
        }
        public int GetSize()
        {
            return Index;
        }
        public string GetString()
        {
            Array.Resize<byte>(ref _buffer, Index);
            return Encoding.Default.GetString(_buffer);
        }
    }
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
            _conn.Send(data_out);
            return true;
        }
        public bool Recv()
        {
            byte[] recv_buffer = new byte[NET_RECV_SIZE];
            int recvBytes;

            NetRecvBuffer rb = new NetRecvBuffer();
            RedisSerializer rs = new RedisSerializer();

            do
            {
                recvBytes = _conn.Receive(recv_buffer);
                rb.Add(recv_buffer, recvBytes);

            } while (recvBytes == NET_RECV_SIZE);

            rs.Deserialize(rb.GetBuffer(), rb.GetSize(), ref _RecvString);

            return true;
        }
//       public REDIS_RESPONSE_TYPE Request(RESPMaker req)
         public String  Request(RESPMaker req)
        {
            // Reqest & response
            Send(req.Make().ToString());
            Recv();
            return _RecvString;
            // analyse & build data struture to make useful.
            /*rr.parse(_RecvString);
            return rr.response_type;*/
        }
    }
    public class RESPToken
    {
        private string _token;
        public RESPToken(float val)
        {
            StringBuilder result = new StringBuilder();
            result.Append(":");
            result.Append(val.ToString());
            result.Append("\r\n");
            _token = result.ToString();
        }
        public RESPToken(int val)
        {
            StringBuilder result = new StringBuilder();
            result.Append(":");
            result.Append(val.ToString());
            result.Append("\r\n");
            _token = result.ToString();
        }
        public RESPToken(string val, bool simple = false)// 기본적으로 bulk string으로 제작. simple string이 필요할 경우 simple를 true로 세팅 
        {
            StringBuilder result = new StringBuilder();

            if (simple)
            {
                result.Append("+");
                result.Append(val.ToString());
                result.Append("\r\n");
            }
            else
            {
                result.Append("$");
                result.Append(val.Length.ToString());
                result.Append("\r\n");
                result.Append(val.ToString());
                result.Append("\r\n");
            }
            _token = result.ToString();
        }
        public string GetToken()
        {
            return _token.ToString();
        }
    }
    public class RESPMaker
    {
        private int _count;
        private List<RESPToken> _tokens;

        public RESPMaker()
        {
            _tokens = new List<RESPToken>();
            _count = 0;
        }
        public void Init()
        {
            _tokens.Clear();
            _count = 0;
        }
        public void Add(RESPToken token)
        {
            _tokens.Add(token);
            _count++;
        }
        public string Make()
        {
            StringBuilder result = new StringBuilder();
            if (_count == 0)
            {
                result.Append("*0\r\n");
            }
            else
            {
                result.Append("*");
                result.Append(_count.ToString());
                result.Append("\r\n");

                foreach (RESPToken t in _tokens)
                {
                    result.Append(t.GetToken());
                }
            }
            return result.ToString();
        }
    }
} // end of namepsace Redis 
} // end of namespace Syncnet 
