using UnityEngine;
namespace ChatManagerUtility
{
    public class MessageTypeHandler
    {
        readonly string InternalMsg;
        readonly Time InternalMsgTime;
        readonly int InternalCharacterLimit;

        public string Msg{
            get 
            { 
              if(InternalMsg.Length < InternalCharacterLimit)
                {
                  return InternalMsg;
              }
              return InternalMsg.Substring(0, InternalCharacterLimit);
            }
        }
        public Time MsgTime
        {
            get
            {
                return InternalMsgTime;
            }
        }

        public MessageTypeHandler(string currentMessage, Time assignedMsgTime, int characterLimit){
            InternalMsg = currentMessage;
            InternalMsgTime = assignedMsgTime;
            InternalCharacterLimit = characterLimit;
        }

        
        
    }
}