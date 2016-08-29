using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncnet
{
    namespace RedisLib
    {
        public class RESPNode
        {
            public String Value;
            public REDIS_RESPONSE_TYPE Type;

            public int TotalCount;
            public int CurrentCount;
            
            public RESPNode Child;
            public RESPNode Parent;
            public RESPNode Next;
            public RESPNode Prev;

            public RESPNode()
            {                
                Child = null;
                Parent = null;
                Next = null;
                Prev = null;
                TotalCount = 1;
                CurrentCount = 0;
            }
            public void SetData(REDIS_RESPONSE_TYPE NewType, String NewValue, int TCount, int CCount)
            {
                Value = NewValue;
                Type = NewType;
                TotalCount = TCount;
                CurrentCount = CCount;
            }

            public RESPNode GoBack(RESPNode Head)
            {
                RESPNode Mover = this;
                // 노드 리스트의 끝까지 간 경우에만 
                if (TotalCount != CurrentCount) return null;
                while(true)
                {
                    if( Mover.Prev == null )
                    {
                        if (Mover.Parent == null) return Mover; // 노드 적재가 다 끝났다. 
                        else return Mover.Parent;
                    }
                    else
                    {
                        if (Mover.Prev == Head) return Mover;
                        Mover = Mover.Prev;
                    }
                }
            }
        
            public RESPNode GetParent()
            {
                return Parent;
            }
            public RESPNode GetChild()
            {
                return Child;
            }
            public RESPNode GetNext()
            {
                return Next;
            }
            public RESPNode GetPrev()
            {
                return Prev;
            }
            public void AddChild(ref RESPNode NewChild)
            {
                Child = NewChild;
                NewChild.Parent = this;                
            }
            public void AddNext(ref RESPNode NewNext)
            {
                Next = NewNext;
                NewNext.Prev = this;
            }
            
        }
    }
}