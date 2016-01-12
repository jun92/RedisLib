using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{ 
namespace RedisLib
{
    /// <summary>
    /// 네트워크로부터 받은 데이타를 저장하기 위한 가변 byte배열 관리 클래스 
    /// </summary>
    public class NetRecvBuffer
    {
        /// <summary>
        /// 내용을 저장할 버퍼 
        /// </summary>
        private byte[] _buffer;
        /// <summary>
        /// 버퍼의 인덱스, 현재 위치 
        /// </summary>
        private int Index;
        private const int BUFFER_SIZE = 4096;
        /// <summary>
        /// 기본 초기화, 기본 버퍼 크기는 1024로 지정된다. 
        /// </summary>
        public NetRecvBuffer()
        {
            _buffer = new Byte[BUFFER_SIZE];
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
}
}
