using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syncnet
{ 
namespace RedisLib
{
    public enum REDIS_NODE_ADD_DIRECTION
    {
        NEXT = 1,
        CHILD = 2
    }
    public class RedisRESP2Class
    {
        public string _error_msg;
        public int result_int;
        public string result_string;
        public List<String> result_array;
        public List<dynamic> result_nested;
        public REDIS_RESPONSE_TYPE response_type;

        private StringBuilder crumb_buffer;
        private RESPNode StartNode; // It should be empty, only use StartNode.Next as pointing first node.
        private RESPNode CurNode;
        
        public RedisRESP2Class()
        {
            result_array = new List<String>();
            result_nested = new List<dynamic>();
            crumb_buffer = new StringBuilder();
            StartNode = new RESPNode();
            CurNode = null;
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

            //if (-1 == token.IndexOf('\n')) return (int)REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
            String[] t = token.Split(d, 2);
            token = t[1]; // 이미 처리한 스트링은 빼버리고 나머지 부분을 남겨준다. 
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
        public REDIS_RESPONSE_TYPE parseR(String RESP)
        {
            result_nested.Clear();

            crumb_buffer.Append(RESP.ToString());

            String param = crumb_buffer.ToString();

            response_type = MakeNode(ref param);
            crumb_buffer.Clear();
            crumb_buffer.Append(param);
            //response_type = parse_recursiveR(ref param, ref result_nested);
            return response_type;
        }
        public void AddNode(REDIS_NODE_ADD_DIRECTION Direction, ref RESPNode CurNode, out RESPNode NewNode, REDIS_RESPONSE_TYPE type, String TokenValue, ref int ArraySize)
        {
            NewNode = new RESPNode();
            if( Direction == REDIS_NODE_ADD_DIRECTION.NEXT ) // 항목의 열거일 경우 
            {
                CurNode.AddNext(ref NewNode);
                CurNode = CurNode.Next;
                CurNode.SetData(type, TokenValue, CurNode.Prev.TotalCount, CurNode.Prev.CurrentCount + 1);
            }
            else // 새로운 배열  요소가 추가됐을 경우 
            {   
                CurNode.AddChild(ref NewNode);
                CurNode = CurNode.Child;
                CurNode.SetData(type, TokenValue, ArraySize, 0);
            }
        }
        
        public REDIS_RESPONSE_TYPE MakeNode(ref String param)
        {
            bool IsPacketEnd = false; 
            String TokenValue = "";             
            REDIS_TOKEN_RETURTN_TYPE TType;            

            int ArraySize = 0;
            if( CurNode == null )
            {
                CurNode = StartNode;
            }
            while( true )
            {
                TType = GetNextToken(ref param, ref TokenValue);
            
                switch( TType)
                {
                    case REDIS_TOKEN_RETURTN_TYPE.INT:                    
                        {
                            RESPNode NewNode;// = new RESPNode();                           
                            AddNode(REDIS_NODE_ADD_DIRECTION.NEXT, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.INT, TokenValue,  ref ArraySize);                
                        }                    
                        break;
                    case REDIS_TOKEN_RETURTN_TYPE.SSTRING:
                        {
                            RESPNode NewNode;// = new RESPNode();
                            AddNode(REDIS_NODE_ADD_DIRECTION.NEXT, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.SSTRING, TokenValue, ref ArraySize);                            
                        }                    
                        break;
                    case REDIS_TOKEN_RETURTN_TYPE.ERROR:
                        {
                            RESPNode NewNode;// = new RESPNode();
                            AddNode(REDIS_NODE_ADD_DIRECTION.NEXT, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.ERROR, TokenValue, ref ArraySize);
                        }
                        break;
                    case REDIS_TOKEN_RETURTN_TYPE.BSTRING: // ex) "$4\r\n" 
                        {
                            int StringLength = int.Parse(TokenValue);
                            if(-1 == StringLength ) // handle "$-1\r\n"
                            {
                                // add null string. 
                                RESPNode NewNode;// = new RESPNode();
                                AddNode(REDIS_NODE_ADD_DIRECTION.NEXT, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.BSTRING, null, ref ArraySize);                                
                            }
                        }
                        break;
                    case REDIS_TOKEN_RETURTN_TYPE.BSTRINGBODY: // ex) "$4\r\nABCD\r\n" => "ABCD\r\n\" 
                        {
                            RESPNode NewNode;// = new RESPNode();
                            AddNode(REDIS_NODE_ADD_DIRECTION.NEXT, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.BSTRING, TokenValue, ref ArraySize);
                        }
                        break;
                    case REDIS_TOKEN_RETURTN_TYPE.ARRAY:
                        {
                            ArraySize = int.Parse(TokenValue);
                            RESPNode NewNode;// = new RESPNode();
                            if( ArraySize == -1 )
                            {
                                // nil array 
                                AddNode(REDIS_NODE_ADD_DIRECTION.NEXT, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.ARRAY, null, ref ArraySize);                            
                            }
                            else if( ArraySize == 0)
                            {
                                ArraySize = 0;
                                AddNode(REDIS_NODE_ADD_DIRECTION.NEXT, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.ARRAY, "", ref ArraySize);
                                AddNode(REDIS_NODE_ADD_DIRECTION.CHILD, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.ARRAY, "", ref ArraySize);
                            }
                            else // 코드를 좀 더 명확하게 보이기 위해 중복 코드로. 
                            {
                                AddNode(REDIS_NODE_ADD_DIRECTION.NEXT, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.ARRAY, "", ref ArraySize);
                                AddNode(REDIS_NODE_ADD_DIRECTION.CHILD, ref CurNode, out NewNode, REDIS_RESPONSE_TYPE.ARRAY, "", ref ArraySize);                                
                            }                            
                           
                            ArraySize = 0;

                        }
                        break;
                    case REDIS_TOKEN_RETURTN_TYPE.NOT_ENOUGH_DATA:
                        return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
                }                
                // 반복 
                while( true )
                {
                    if (CurNode.TotalCount == CurNode.CurrentCount)
                    {
                        CurNode = CurNode.GoBack(StartNode);

                        if (CurNode == StartNode.Next)
                        {
                            IsPacketEnd = true;
                            break;
                        }
                    }
                    else
                        break;
                        
                }
                if (IsPacketEnd) break;                
                if (param == "") return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
            }

            // 자료 구조 읽어서 result_nested에 적재 
            if (IsPacketEnd)
            {
                RESPNode it = StartNode.Next;

                while( it != null )
                {
                    if (it.Type == REDIS_RESPONSE_TYPE.ARRAY)
                    {
                        List<dynamic> NewNode = new List<dynamic>();
                        it = GetNodeArray(it, ref NewNode);
                        result_nested.Add(NewNode);
                    }
                    else
                    {
                        //Console.WriteLine("type:[{0}],value:[{1}]({2}/{3})", it.Type.ToString(), it.Value.ToString(), it.CurrentCount, it.TotalCount);
                        AddResultNested(ref result_nested, it.Type, it.Value);
                    }   
                    it = it.Next;
                }
            }
            return StartNode.Next != null ? StartNode.Next.Type : REDIS_RESPONSE_TYPE.ERROR;
        }
        public void AddResultNested(ref List<dynamic> node, REDIS_RESPONSE_TYPE type, String Value )
        {
            switch(type)
            {
                case REDIS_RESPONSE_TYPE.BSTRING:
                case REDIS_RESPONSE_TYPE.SSTRING:
                case REDIS_RESPONSE_TYPE.ERROR:
                    node.Add(Value);
                    break;
                case REDIS_RESPONSE_TYPE.INT:
                    node.Add(int.Parse(Value));
                    break;                
            }
        }
        public RESPNode GetNodeArray(RESPNode CurNode, ref List<dynamic> NewNode)
        {
            RESPNode it = CurNode;
            int TotalCount;           

            it = it.Child;
            TotalCount = it.TotalCount;
            it = it.Next;
            
            while( it != null )
            {
                if (it.Type == REDIS_RESPONSE_TYPE.ARRAY)
                {
                    List<dynamic> NewChildNode = new List<dynamic>();
                    it = GetNodeArray(it, ref NewChildNode);
                    NewNode.Add(NewChildNode);
                }
                else
                {
                    //Console.WriteLine("type:[{0}],value:[{1}]({2}/{3})", it.Type.ToString(), it.Value.ToString(), it.CurrentCount, it.TotalCount);
                    AddResultNested(ref NewNode, it.Type, it.Value);
                }
                it = it.Next;
            }
            return CurNode;
        }

        public REDIS_TOKEN_RETURTN_TYPE GetNextToken(ref String Param, ref String Token)
        {
            REDIS_TOKEN_RETURTN_TYPE RetVal = REDIS_TOKEN_RETURTN_TYPE.ERROR;
            String[] Delimeter = { "\r\n" };
            String[] Tokens = Param.Split(Delimeter, 2, StringSplitOptions.None); // this action strips \r\n. 
            if( Tokens.Length < 2 )
            {
                RetVal = REDIS_TOKEN_RETURTN_TYPE.NOT_ENOUGH_DATA;                
            }
            else
            {
                if (Tokens[0].StartsWith(":"))
                {
                    RetVal = REDIS_TOKEN_RETURTN_TYPE.INT;
                    Token = Tokens[0].Substring(1);
                }
                else if (Tokens[0].StartsWith("+"))
                {
                    RetVal = REDIS_TOKEN_RETURTN_TYPE.SSTRING;
                    Token = Tokens[0].Substring(1);
                }
                else if (Tokens[0].StartsWith("-"))
                {
                    RetVal = REDIS_TOKEN_RETURTN_TYPE.ERROR;
                    Token = Tokens[0].Substring(1);
                }
                else if (Tokens[0].StartsWith("$"))
                {
                    RetVal = REDIS_TOKEN_RETURTN_TYPE.BSTRING;
                    Token = Tokens[0].Substring(1);
                }
                else if (Tokens[0].StartsWith("*"))
                {
                    RetVal = REDIS_TOKEN_RETURTN_TYPE.ARRAY;
                    Token = Tokens[0].Substring(1);
                }
                else
                {
                    RetVal = REDIS_TOKEN_RETURTN_TYPE.BSTRINGBODY;
                    Token = Tokens[0];
                }
                Param = Tokens[1];
            }            
            return RetVal;
        }
        
        public String GetRESPToken(ref String Param)
        {
            String[] delimeter = { "\r\n" };            
            String[] token = Param.Split(delimeter, 2, StringSplitOptions.None); // this action strips \r\n. 
            if( token.Length < 2)
            {
                return null;
            }
            else
            {
                String NewToken = token[0];
                String SizeString = NewToken.Substring(1);
                int Size = int.Parse(SizeString);

                // 배열이나 문자열의 경우 그 다음 한개의 \r\n까지 왔는지 체크해서 리턴한다.(RESPNode생성 문제때문에)
                // 예외 : $의 경우 -1일 경우(= "$-1\r\n"), *의 경우 0일 경우(="*0\r\n"). 

                if( (NewToken.StartsWith("*") && Size != 0) || (NewToken.StartsWith("$") && Size != -1 ))
                {   
                    String[] SecondTryToken = token[1].Split(delimeter, 2, StringSplitOptions.None);
                    if (SecondTryToken.Length < 2) { return null; }
                    else
                    {
                        StringBuilder RetString = new StringBuilder();
                        RetString.Append(token[0].ToString());
                        RetString.Append("\r\n");
                        RetString.Append(SecondTryToken[0].ToString());

                        Param = SecondTryToken[1];
                        return RetString.ToString();
                    }
                }
                Param = token[1];
                return token[0];
            }
        }
        public REDIS_RESPONSE_TYPE parse_recursiveR(ref String p, ref List<dynamic> retval)
        {
            p = p.Replace("\r", "");

            if (p.StartsWith(":"))  // int이므로 
            {
                if (-1 == p.IndexOf('\n')) return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
                retval.Add(getInteger(ref p)); 
                return REDIS_RESPONSE_TYPE.INT; 
            }
            if (p.StartsWith("+"))  // Simple String 
            {
                if (-1 == p.IndexOf('\n')) return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
                retval.Add(getSString(ref p)); 
                return REDIS_RESPONSE_TYPE.SSTRING; 
            }
            if (p.StartsWith("-")) // error 
            {
                if (-1 == p.IndexOf('\n')) return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
                retval.Add(getError(ref p)); 
                return REDIS_RESPONSE_TYPE.ERROR; 
            }
            if (p.StartsWith("$")) // String  
            { 
                // \n이 두개가 있어야한다. 
                int pos;

                if (-1 != (pos = p.IndexOf('\n')) && -1 != p.IndexOf('\n', pos + 1))
                {
                    retval.Add(getBString(ref p));
                    return REDIS_RESPONSE_TYPE.BSTRING;
                }
                else return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
            } 
            if (p.StartsWith("*")) // array 
            {
                // 배열이라면 배열 갯수를 구해
                // 배열 갯수 만큼 반복하며 
                int array_size;
                char[] d = { '\n' };

                if (-1 == p.IndexOf('\n')) return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;

                String[] tokens = p.Split(d, 2); // tokens[0]에 갯수, tokens[1]에 데이타가 있다. 
                array_size = int.Parse(tokens[0].Substring(1)); // *표 제거후 숫자로 변경 

                for (int i = 0; i < array_size; i++)
                {
                    if (tokens[1].StartsWith(":"))
                    {
                        if (-1 == tokens[1].IndexOf('\n')) return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
                        retval.Add(getInteger(ref tokens[1]));
                        continue;
                    }
                    else if (tokens[1].StartsWith("+"))
                    {
                        if (-1 == tokens[1].IndexOf('\n')) return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
                        retval.Add(getSString(ref tokens[1]));
                        continue;
                    }
                    else if (tokens[1].StartsWith("$"))
                    {
                        int pos;

                        if (-1 != (pos = tokens[1].IndexOf('\n')) && -1 != tokens[1].IndexOf('\n', pos + 1))
                        {
                            retval.Add(getBString(ref tokens[1]));
                            continue;
                        }
                        else return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
                    }
                    else if (tokens[1].StartsWith("*"))
                    {
                        List<dynamic> node = new List<dynamic>();
                        parse_recursive(ref tokens[1], ref node);
                        retval.Add(node);
                        continue;
                    }
                    else return REDIS_RESPONSE_TYPE.NOT_ENOUGH_DATA;
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