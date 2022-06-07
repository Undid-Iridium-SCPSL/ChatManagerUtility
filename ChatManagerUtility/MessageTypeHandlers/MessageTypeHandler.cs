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

        public MessageTypeHandler(string currentMessage, float assignedMsgTime, int characterLimit, MessageType messageType, string chatColor)
        {
            
            InternalMsgTime = assignedMsgTime - 0.1000f;
            InternalCharacterLimit = characterLimit;
            InternalMsgType = messageType;

            if (currentMessage.Length > InternalCharacterLimit)
            {
                currentMessage = currentMessage.Substring(0, InternalCharacterLimit);
            }
            string temp = chatColor + "<size=40%>" + currentMessage + "</color>";
            InternalMsg = temp;
            //Log.Info($"What in the HELL was the current msg {InternalMsg}  vs temp {temp} and chat color {chatColor} and original msg {currentMessage}");
        }




    }
}