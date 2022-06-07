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

        public string ConsoleMsg { get; set; }

        public string Msg
        {
            get 
            { 
              return InternalMsg;
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

        public MessageTypeHandler(string currentMessage, float assignedMsgTime, int characterLimit, MessageType messageType, string chatColor, string msgSize)
        {
            
            InternalMsgTime = assignedMsgTime - 0.1000f;
            InternalCharacterLimit = characterLimit;
            InternalMsgType = messageType;
            ConsoleMsg = currentMessage.Substring(0, 256);
            if (currentMessage.Length > InternalCharacterLimit)
            {
                currentMessage = currentMessage.Substring(0, InternalCharacterLimit);
            }
            string temp = chatColor + msgSize + currentMessage + "</color>";
            InternalMsg = temp;
            //Log.Info($"What in the HELL was the current msg {InternalMsg}  vs temp {temp} and chat color {chatColor} and original msg {currentMessage}");
        }




    }
}