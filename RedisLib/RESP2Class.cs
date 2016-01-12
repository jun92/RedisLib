using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisRESP2Class
    {
        public string _error_msg;
        public int result_int;
        public string result_string;
        public List<String> result_array;
        public List<dynamic> result_nested;
        public REDIS_RESPONSE_TYPE response_type;

        public RedisRESP2Class()
        {
            result_array = new List<String>();
            result_nested = new List<dynamic>();
        }

        public void getAsDictionary(ref Dictionary<String, String> dic)
        {

            if (result_nested.Count == 0) return;
            if ((result_nested.Count % 2) != 0) return;
            for (int i = 0; i < result_nested.Count; i += 2)
            {
                dic.Add(result_nested[i].ToString(), result_nested[i + 1].ToString());
            }
        }
        public void getAsLists(ref List<String> list)
        {
            if (result_nested.Count == 0) return;
            for (int i = 0; i < result_nested.Count; i++)
            {
                list.Add(result_nested[i].ToString());
            }
        }
        public void getAsNestedArray(ref List<dynamic> narray)
        {
            narray = result_nested;
            
        }
        public String getAsString()
        {
            if (result_nested.Count != 1) return "";
            return result_nested[0].ToString();

        }
        public int getAsInt()
        {
            if (result_nested.Count != 1) return 0;
            return result_nested[0];
        }

        private int getInteger(ref String token)
        {
            char[] d = { '\n' };
            String[] t = token.Split(d, 2);
            token = t[1];
            return int.Parse(t[0].Substring(1));
        }
        private String getLiteral(ref String token)
        {
            char[] d = { '\n' };
            String[] t = token.Split(d, 2);
            token = t[1];
            return t[0].Substring(1);
        }
        private String getSString(ref String token)
        {
            return getLiteral(ref token);

        }
        private String getBString(ref String token)
        {
            char[] d = { '\n' };
            String[] t = token.Split(d, 2); // 0-크기,  1-실제 데이타 
            String[] bst = t[1].Split(d, 2);
            if( bst.Length != 2) 
            {
                token = "";
            }
            else
            {
                token = bst[1];
            }

            return bst[0].ToString();
        }
        private String getError(ref String token)
        {
            return getLiteral(ref token);
        }

        public void parse(String RESP)
        {
            result_nested.Clear();
            response_type = parse_recursive(ref RESP, ref result_nested);
        }
        public REDIS_RESPONSE_TYPE parse_recursive(ref String p, ref List<dynamic> retval)
        {
            p = p.Replace("\r", "");

            if (p.StartsWith(":")) { retval.Add(getInteger(ref p)); return REDIS_RESPONSE_TYPE.INT; } // int이므로 
            if (p.StartsWith("+")) { retval.Add(getSString(ref p)); return REDIS_RESPONSE_TYPE.SSTRING; }// String 
            if (p.StartsWith("-")) { retval.Add(getError(ref p)); return REDIS_RESPONSE_TYPE.ERROR; }  // error 
            if (p.StartsWith("$")) { retval.Add(getBString(ref p)); return REDIS_RESPONSE_TYPE.BSTRING; } // String 
            if (p.StartsWith("*"))
            {
                // 배열이라면 배열 갯수를 구해
                // 배열 갯수 만큼 반복하며 
                int array_size;
                char[] d = { '\n' };

                String[] tokens = p.Split(d, 2); // tokens[0]에 갯수, tokens[1]에 데이타가 있다. 
                array_size = int.Parse(tokens[0].Substring(1)); // *표 제거후 숫자로 변경 

                for (int i = 0; i < array_size; i++)
                {
                    if (tokens[1].StartsWith(":")) { retval.Add(getInteger(ref tokens[1])); continue; }
                    if (tokens[1].StartsWith("+")) { retval.Add(getSString(ref tokens[1])); continue; }
                    if (tokens[1].StartsWith("$")) { retval.Add(getBString(ref tokens[1])); continue; }
                    if (tokens[1].StartsWith("*"))
                    {
                        List<dynamic> node = new List<dynamic>();
                        parse_recursive(ref tokens[1], ref node);
                        retval.Add(node);
                        continue;
                    }
                }
                p = tokens[1];
                return REDIS_RESPONSE_TYPE.ARRAY;
            }
            return REDIS_RESPONSE_TYPE.ERROR;
        }
        public bool IsCompletedPacket(String buffer)
        {            
            String RESP = buffer;             
            return IsRESPTokenCompleted(ref RESP);
        }
        public bool IsRESPTokenCompleted(ref String buffer)
        {
            if (buffer.StartsWith(":")) { return buffer.IndexOf("\r\n") == -1 ? false : true; } // integer 
            if (buffer.StartsWith("-")) { return buffer.IndexOf("\r\n") == -1 ? false : true; } // error 
            if (buffer.StartsWith("+")) { return buffer.IndexOf("\r\n") == -1 ? false : true; } // simple string (길이 정보가 없음) 
            if (buffer.StartsWith("$")) 
            { 
                int pos; 
                if( -1 != (pos =  buffer.IndexOf("\r\n")) )
                {
                    if( -1 == buffer.IndexOf("\r\n", pos+2) ) return false;                    
                    else return true;
                }
                else return false;
            } // bulk string (길이가 있는) 
            if (buffer.StartsWith("*")) 
            {
                int i;
                String[] d = {"\r\n"};
                String[] tokens = buffer.Split(d, 2, StringSplitOptions.None);
                int arrayCount = int.Parse( tokens[0].Substring(1));

                buffer = tokens[1];

                if (arrayCount == 0) return true;

                for( i = 0; i < arrayCount; i ++ )
                {
                    if (buffer.StartsWith(":") && buffer.IndexOf("\r\n") == -1)
                    {
                        buffer = buffer.Substring(buffer.IndexOf("\r\n"));
                        return false;
                    }
                    if (buffer.StartsWith("-") && buffer.IndexOf("\r\n") == -1)
                    {
                        buffer = buffer.Substring(buffer.IndexOf("\r\n"));
                        return false;
                    }
                    if (buffer.StartsWith("+") && buffer.IndexOf("\r\n") == -1)
                    {
                        buffer = buffer.Substring(buffer.IndexOf("\r\n"));
                        return false;
                    }
                    if( buffer.StartsWith("$"))
                    {                        
                        // \r\n으로 구분되는 길이:데이타가 있어야한다. 
                        int pos;
                        if( -1 == (pos = buffer.IndexOf("\r\n"))) return false;
                        buffer = buffer.Substring(buffer.IndexOf("\r\n") + 2);
                        if (-1 == (pos = buffer.IndexOf("\r\n"))) return false;
                        buffer = buffer.Substring(buffer.IndexOf("\r\n")+2);

                    }
                    if( buffer.StartsWith("*"))
                    {
                        if( ! IsRESPTokenCompleted(ref buffer) ) return false; 
                    }
                    if (buffer.Length == 0) break;
                }
                
                if (i+1 != arrayCount) return false;
            } // 배열 
            return true;
        }
    }


    
}

}