using ChatManagerUtility.Configs;
using UnityEngine;
namespace ChatManagerUtility
{
    public class MessageTypeHandler
    {
        readonly string InternalMsg;
        readonly float InternalMsgTime;
        readonly int InternalCharacterLimit;
        readonly MessageType InternalMsgType;

        public string Msg
        {
            get 
            { 
              if(InternalMsg.Length < InternalCharacterLimit)
                {
                  return InternalMsg;
              }
              return InternalMsg.Substring(0, InternalCharacterLimit);
            }
        }
        public float MsgTime
        {
            get
            {
                return InternalMsgTime;
            }
        }

        public MessageType MsgType => InternalMsgType;

        public MessageTypeHandler(string currentMessage, float assignedMsgTime, int characterLimit, MessageType messageType, string chatColor)
        {
            
            InternalMsgTime = assignedMsgTime;
            InternalCharacterLimit = characterLimit;
            InternalMsgType = messageType;
            InternalMsg = chatColor + currentMessage;
        }

        
        
    }
}