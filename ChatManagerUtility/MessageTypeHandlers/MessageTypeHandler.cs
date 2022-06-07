using ChatManagerUtility.Configs;
using Exiled.API.Features;
using UnityEngine;
namespace ChatManagerUtility
{
    public class MessageTypeHandler
    {
        string InternalMsg { get; set; }
        float InternalMsgTime { get; set; }
        int InternalCharacterLimit { get; set; }
        MessageType InternalMsgType { get; set; }

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
            string temp = chatColor + currentMessage + "</color>";
            InternalMsg = temp;
            Log.Info($"What in the HELL was the current msg {InternalMsg}  vs temp {temp} and chat color {chatColor} and original msg {currentMessage}");
        }




    }
}