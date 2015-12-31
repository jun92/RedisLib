using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet
{ 
namespace RedisLib
{
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
        public void Add(String token)
        {
            _tokens.Add(new RESPToken(token));
            _count++;
        }
        public void Add(int token)
        {
            _tokens.Add(new RESPToken(token));
            _count++;
        }
        public void Add(float token)
        {
            _tokens.Add(new RESPToken(token));
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
}
}