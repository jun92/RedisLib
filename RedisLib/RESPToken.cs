using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{ 
namespace RedisLib
{
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
}
}
