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
    }
}
